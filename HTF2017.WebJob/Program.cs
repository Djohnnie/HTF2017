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
                    Team team = await dbContext.Androids.Where(x => x.Id == dataRequest.AndroidId)
                        .Select(x => x.Team).SingleOrDefaultAsync();
                    if (team != null)
                    {
                        Console.WriteLine($"[ HTF2017 - Processing datarequest for '{team.Name}'. ]");

                        SensoryData data = new SensoryData
                        {
                            AndroidId = dataRequest.AndroidId,
                            Longitude = dataRequest.Location ? 9 : (Double?)null,
                            Lattitude = dataRequest.Location ? 9 : (Double?)null,
                            Crowd = dataRequest.Crowd ? 100 : (Byte?)null,
                            Mood = dataRequest.Mood ? 100 : (Byte?)null,
                            Relationship = dataRequest.Relationship ? 100 : (Byte?)null,
                            TimeStamp = DateTime.UtcNow
                        };

                        await dbContext.SensoryData.AddAsync(data);
                        dataRequest.Fulfilled = true;
                        await dbContext.SaveChangesAsync();
                        Console.WriteLine($"[ HTF2017 - datarequest for '{team.Name}' processed and fulfilled. ]");
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
    }
}