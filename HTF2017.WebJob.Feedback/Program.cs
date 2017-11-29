using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTF2017.DataAccess;
using HTF2017.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using RestSharp;

namespace HTF2017.WebJob.Feedback
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
                        Console.WriteLine("[ HTF2017 - WebJOB Feedback - 'HandlePendingSensoryData' ]");
                        await HandlePendingSensoryData();

                        await Task.Delay(1000);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ HTF2017 - EXCEPTION - '{ex.Message}'! ]");
                    }
                }
            }).Wait();
        }

        private static async Task HandlePendingSensoryData()
        {
            using (HtfDbContext dbContext = new HtfDbContext())
            {
                Console.WriteLine("[ HTF2017 - Getting pending sensory data... ]");
                List<SensoryData> data = await dbContext.SensoryData
                    .Where(x => !x.Sent).OrderBy(x => x.TimeStamp).ToListAsync();
                Console.WriteLine(
                    $"[ HTF2017 - {(data.Count > 0 ? $"{data.Count}" : "no")} pending sensory data found. ]");

                foreach (SensoryData d in data)
                {
                    Team team = await dbContext.Androids.Where(x => x.Id == d.AndroidId)
                        .Select(x => x.Team).SingleOrDefaultAsync();
                    if (team != null)
                    {
                        Console.WriteLine($"[ HTF2017 - Processing data for '{team.Name}'. ]");
                        Boolean received = false;

                        try
                        {
                            RestClient client = new RestClient(team.FeedbackEndpoint);
                            RestRequest request = new RestRequest(Method.POST);
                            FeedbackRequestDto feedback = new FeedbackRequestDto
                            {
                                AndroidId = d.AndroidId,
                                Longitude = d.Longitude,
                                Lattitude = d.Lattitude,
                                Crowd = d.Crowd,
                                Mood = d.Mood,
                                Relationship = d.Relationship,
                                TimeStamp = d.TimeStamp,
                                AutonomousRequest = d.AutonomousRequested
                            };
                            request.AddJsonBody(feedback);
                            FeedbackResponseDto response = await client.PostTaskAsync<FeedbackResponseDto>(request);
                            if (response != null && response.TeamId == team.Id)
                            {
                                if (d.AutonomousRequested)
                                {
                                    team.Score += 10;
                                }
                                else
                                {
                                    team.Score += 20;
                                }
                                received = true;
                            }
                            else
                            {
                                team.Score--;
                            }
                        }
                        catch
                        {
                            team.Score--;
                        }

                        d.Sent = true;
                        d.Received = received;
                        await dbContext.SaveChangesAsync();
                        Console.WriteLine($"[ HTF2017 - data for '{team.Name}' {(received ? "successfully sent" : "failed to send")}. ]");
                    }
                    else
                    {
                        Console.WriteLine($"[ HTF2017 - PANIC - No team found for android '{d.AndroidId}'! ]");
                    }
                    await Task.Delay(100);
                }
            }
        }
    }
}