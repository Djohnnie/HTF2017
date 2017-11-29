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
                        Console.WriteLine($"[ HTF2017 - Processing datarequest for '{android.Team.Name}'. ]");

                        SensoryData data = new SensoryData
                        {
                            AndroidId = dataRequest.AndroidId,
                            Longitude = dataRequest.Location ? 9 : (Double?)null,
                            Lattitude = dataRequest.Location ? 9 : (Double?)null,
                            Crowd = dataRequest.Crowd ? 100 : (Int32?)null,
                            Mood = dataRequest.Mood ? 100 : (Byte?)null,
                            Relationship = dataRequest.Relationship ? 100 : (Byte?)null,
                            TimeStamp = DateTime.UtcNow
                        };

                        await dbContext.SensoryData.AddAsync(data);
                        dataRequest.Fulfilled = true;

                        Boolean isCompromised = IsAndroidCompromised(android.AutoPilot,
                        android.LocationSensorAccuracy, android.CrowdSensorAccuracy,
                        android.MoodSensorAccuracy, android.RelationshipSensorAccuracy);
                        if (isCompromised)
                        {
                            android.Compromised = true;
                            android.Team.Score -= 1000;
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
            await Task.Delay(1000);
        }

        private static Boolean IsAndroidCompromised(AutoPilot autoPilot,
            SensorAccuracy location, SensorAccuracy crowd,
            SensorAccuracy mood, SensorAccuracy relationship)
        {
            // 0% - 100% chance of compromised!
            // AutoPilot Level-1: 0% + sensors = 15%
            // AutoPilot Level-2: 10% + sensors = [0% - 70%]
            // AutoPilot Level-3: 20% + sensors = [0% - 80%]
            // Sensor Off: 0%
            // Sensor Low: 5%
            // Sensor Medium: 10%
            // Sensor High: 15%
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
                    return 10;
                case AutoPilot.Level3:
                    return 20;
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
                    return 5;
                case SensorAccuracy.MediumAccuracySensor:
                    return 10;
                case SensorAccuracy.HighAccuracySensor:
                    return 15;
            }
            return 100;
        }
    }
}