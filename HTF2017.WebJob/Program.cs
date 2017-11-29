using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTF2017.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace HTF2017.WebJob
{
    class Program
    {
        private static readonly Random _randomGenerator = new Random();

        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        Console.WriteLine("[ HTF2017 - WebJOB - 'HandlePendingRequests' ]");
                        await HandlePendingRequests();

                        Console.WriteLine("[ HTF2017 - WebJOB - 'HandleAutoFeedback' ]");
                        await HandleAutoFeedback();

                        await Task.Delay(1000);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ HTF2017 - EXCEPTION - '{ex.Message}'! ]");
                    }
                }
            }).Wait();
        }

        private static async Task HandlePendingRequests()
        {
            using (HtfDbContext dbContext = new HtfDbContext())
            {
                Console.WriteLine("[ HTF2017 - Getting pending sensory data requests... ]");
                List<SensoryDataRequest> dataRequests = await dbContext.SensoryDataRequests
                    .Where(x => !x.Fulfilled).OrderBy(x => x.TimeStamp).ToListAsync();
                Console.WriteLine($"[ HTF2017 - {(dataRequests.Count > 0 ? $"{dataRequests.Count}" : "no")} pending sensory data requests found. ]");

                foreach (SensoryDataRequest dataRequest in dataRequests)
                {
                    Android android = await dbContext.Androids.Include(x => x.Team).SingleOrDefaultAsync(x => x.Id == dataRequest.AndroidId);
                    if (android != null)
                    {
                        Location location = await dbContext.Locations.SingleOrDefaultAsync(x => x.Id == android.Team.LocationId);

                        Console.WriteLine($"[ HTF2017 - Processing datarequest for '{android.Team.Name}'. ]");

                        SensoryData previousCrowdSensoryData = await dbContext.SensoryData
                            .Where(x => x.AndroidId == dataRequest.AndroidId && x.Crowd.HasValue)
                            .OrderByDescending(o => o.TimeStamp).FirstOrDefaultAsync();
                        SensoryData previousMoodSensoryData = await dbContext.SensoryData
                            .Where(x => x.AndroidId == dataRequest.AndroidId && x.Mood.HasValue)
                            .OrderByDescending(o => o.TimeStamp).FirstOrDefaultAsync();
                        SensoryData previousRelationshipSensoryData = await dbContext.SensoryData
                            .Where(x => x.AndroidId == dataRequest.AndroidId && x.Relationship.HasValue)
                            .OrderByDescending(o => o.TimeStamp).FirstOrDefaultAsync();

                        SensoryData data = new SensoryData
                        {
                            AndroidId = dataRequest.AndroidId,
                            // https://www.movable-type.co.uk/scripts/latlong.html
                            Longitude = dataRequest.Location ? location.Longitude : (Double?)null,
                            Lattitude = dataRequest.Location ? location.Lattitude : (Double?)null,
                            Crowd = dataRequest.Crowd ? GetCrowd(android, previousCrowdSensoryData) : null,
                            Mood = dataRequest.Mood ? GetMood(android, previousMoodSensoryData) : null,
                            Relationship = dataRequest.Relationship ? GetRelationship(android, previousRelationshipSensoryData) : null,
                            TimeStamp = DateTime.UtcNow,
                            AutonomousRequested = dataRequest.AutonomousRequest
                        };

                        await dbContext.SensoryData.AddAsync(data);
                        dataRequest.Fulfilled = true;

                        Boolean isCompromised = IsAndroidCompromised(android.AutoPilot,
                        android.LocationSensorAccuracy, android.CrowdSensorAccuracy,
                        android.MoodSensorAccuracy, android.RelationshipSensorAccuracy);
                        if (isCompromised)
                        {
                            android.Compromised = true;
                            switch (android.AutoPilot)
                            {
                                case AutoPilot.Level1:
                                    android.Team.Score -= 10;
                                    break;
                                case AutoPilot.Level2:
                                    android.Team.Score -= 100;
                                    break;
                                case AutoPilot.Level3:
                                    android.Team.Score -= 1000;
                                    break;
                            }
                        }

                        await dbContext.SaveChangesAsync();
                        Console.WriteLine($"[ HTF2017 - datarequest for '{android.Team.Name}' processed and fulfilled. ]");
                    }
                    else
                    {
                        Console.WriteLine($"[ HTF2017 - PANIC - No team found for android '{dataRequest.AndroidId}'! ]");
                    }
                }
            }
        }

        private static async Task HandleAutoFeedback()
        {
            using (HtfDbContext dbContext = new HtfDbContext())
            {
                List<Android> androidsToAutonomouslySendData = await dbContext.Androids
                    .Where(x => !x.Compromised && x.AutoPilot != AutoPilot.Level3).ToListAsync();
                foreach (Android android in androidsToAutonomouslySendData)
                {
                    SensoryDataRequest lastAutonomousDataRequest = await dbContext.SensoryDataRequests
                        .Where(x => x.AndroidId == android.Id && x.AutonomousRequest)
                        .OrderByDescending(o => o.TimeStamp).FirstOrDefaultAsync();
                    TimeSpan timeSinceLastAutonomousUpdate =
                        DateTime.UtcNow - (lastAutonomousDataRequest?.TimeStamp ?? DateTime.MinValue);
                    TimeSpan updateThreshold = TimeSpan.MaxValue;
                    switch (android.AutoPilot)
                    {
                        case AutoPilot.Level1:
                            updateThreshold = TimeSpan.FromMinutes(15);
                            break;
                        case AutoPilot.Level2:
                            updateThreshold = TimeSpan.FromMinutes(5);
                            break;
                    }
                    if (timeSinceLastAutonomousUpdate > updateThreshold)
                    {
                        SensoryDataRequest request = new SensoryDataRequest
                        {
                            AndroidId = android.Id,
                            AutonomousRequest = true,
                            Location = true,
                            Crowd = true,
                            Mood = true,
                            Relationship = true,
                            TimeStamp = DateTime.UtcNow
                        };
                        await dbContext.SensoryDataRequests.AddAsync(request);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
        }

        private static Int32? GetCrowd(Android android, SensoryData previousSensoryData)
        {
            TimeSpan sinceLastUpdate = DateTime.UtcNow - (previousSensoryData?.TimeStamp ?? DateTime.UtcNow);
            Int32 maximumDeviation = (Int32)(sinceLastUpdate.TotalMinutes * 1);

            switch (android.CrowdSensorAccuracy)
            {
                case SensorAccuracy.SensorOff:
                    return null;
                case SensorAccuracy.HighAccuracySensor:
                    maximumDeviation += 100;
                    break;
                case SensorAccuracy.MediumAccuracySensor:
                    maximumDeviation += 1000;
                    break;
                case SensorAccuracy.LowAccuracySensor:
                    maximumDeviation += 1000;
                    break;
            }

            Int32 result = (previousSensoryData?.Crowd ?? _randomGenerator.Next(0, 1000000)) + _randomGenerator.Next(-maximumDeviation, maximumDeviation);
            return result < 0 ? 0 : result > 1000000 ? 1000000 : result;
        }

        private static Byte? GetMood(Android android, SensoryData previousSensoryData)
        {
            TimeSpan sinceLastUpdate = DateTime.UtcNow - (previousSensoryData?.TimeStamp ?? DateTime.UtcNow);
            Int32 maximumDeviation = (Int32)(sinceLastUpdate.TotalMinutes * 0.1);

            switch (android.MoodSensorAccuracy)
            {
                case SensorAccuracy.SensorOff:
                    return null;
                case SensorAccuracy.HighAccuracySensor:
                    maximumDeviation += 16;
                    break;
                case SensorAccuracy.MediumAccuracySensor:
                    maximumDeviation += 64;
                    break;
                case SensorAccuracy.LowAccuracySensor:
                    maximumDeviation += 128;
                    break;
            }

            Int32 result = (previousSensoryData?.Mood ?? _randomGenerator.Next(0, 256)) + _randomGenerator.Next(-maximumDeviation, maximumDeviation);
            return result < 0 ? (Byte)0 : result > 255 ? (Byte)255 : (Byte)result;
        }

        private static Byte? GetRelationship(Android android, SensoryData previousSensoryData)
        {
            TimeSpan sinceLastUpdate = DateTime.UtcNow - (previousSensoryData?.TimeStamp ?? DateTime.UtcNow);
            Int32 maximumDeviation = (Int32)(sinceLastUpdate.TotalMinutes * 0.1);

            switch (android.RelationshipSensorAccuracy)
            {
                case SensorAccuracy.SensorOff:
                    return null;
                case SensorAccuracy.HighAccuracySensor:
                    maximumDeviation += 16;
                    break;
                case SensorAccuracy.MediumAccuracySensor:
                    maximumDeviation += 64;
                    break;
                case SensorAccuracy.LowAccuracySensor:
                    maximumDeviation += 128;
                    break;
            }

            Int32 result = (previousSensoryData?.Relationship ?? _randomGenerator.Next(0, 256)) + _randomGenerator.Next(-maximumDeviation, maximumDeviation);
            return result < 0 ? (Byte)0 : result > 255 ? (Byte)255 : (Byte)result;
        }

        private static Boolean IsAndroidCompromised(AutoPilot autoPilot,
            SensorAccuracy location, SensorAccuracy crowd,
            SensorAccuracy mood, SensorAccuracy relationship)
        {
            // 0% - 100% chance of compromised!
            // AutoPilot Level-1: 0% + sensors = 8%
            // AutoPilot Level-2: 3% + sensors = 15%
            // AutoPilot Level-3: 5% + sensors = [5% - 25%]
            // Sensor Off: 0%
            // Sensor Low: 2%
            // Sensor Medium: 3%
            // Sensor High: 5%
            Int32 random = _randomGenerator.Next(0, 101);
            Int32 chance = GetAutoPilotChance(autoPilot)
                + GetSensorAccuracyChance(location)
                + GetSensorAccuracyChance(crowd)
                + GetSensorAccuracyChance(mood)
                + GetSensorAccuracyChance(relationship);
            return random < chance;
        }

        private static Byte GetAutoPilotChance(AutoPilot autoPilot)
        {
            switch (autoPilot)
            {
                case AutoPilot.Level1:
                    return 0;
                case AutoPilot.Level2:
                    return 3;
                case AutoPilot.Level3:
                    return 5;
            }
            return 100;
        }

        private static Byte GetSensorAccuracyChance(SensorAccuracy sensorAccuracy)
        {
            switch (sensorAccuracy)
            {
                case SensorAccuracy.SensorOff:
                    return 0;
                case SensorAccuracy.LowAccuracySensor:
                    return 2;
                case SensorAccuracy.MediumAccuracySensor:
                    return 3;
                case SensorAccuracy.HighAccuracySensor:
                    return 5;
            }
            return 100;
        }
    }
}