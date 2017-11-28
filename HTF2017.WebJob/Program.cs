using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTF2017.DataAccess;
using HTF2017.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RestSharp;

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
                    using (HtfDbContext dbContext = new HtfDbContext())
                    {
                        List<SensoryDataRequest> dataRequests = await dbContext.SensoryDataRequests
                            .Where(x => !x.Fulfilled).OrderBy(x => x.TimeStamp).ToListAsync();
                        foreach (SensoryDataRequest dataRequest in dataRequests)
                        {
                            try
                            {
                                Guid? teamId = await dbContext.Androids.Where(x => x.Id == dataRequest.AndroidId)
                                    .Select(x => x.Id).SingleOrDefaultAsync();
                                if (teamId.HasValue)
                                {
                                    String feedbackUrl = await dbContext.Teams.Where(x => x.Id == teamId)
                                        .Select(x => x.FeedbackEndpoint).SingleOrDefaultAsync();
                                    if (!String.IsNullOrWhiteSpace(feedbackUrl))
                                    {
                                        RestClient client = new RestClient(feedbackUrl);
                                        RestRequest request = new RestRequest(Method.POST);
                                        request.AddJsonBody(new FeedbackRequestDto());
                                        FeedbackResponseDto response =
                                            await client.PostTaskAsync<FeedbackResponseDto>(request);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"ERROR: {ex.Message}");
                            }
                        }
                    }
                }
            }).Wait();
        }
    }
}