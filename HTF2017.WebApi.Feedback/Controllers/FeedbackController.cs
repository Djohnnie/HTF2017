using System;
using System.Linq;
using System.Threading.Tasks;
using HTF2017.DataAccess;
using HTF2017.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HTF2017.WebApi.Feedback.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly HtfDbContext _dbContext;

        public FeedbackController(HtfDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet, Route("feedback")]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost, Route("feedback")]
        public async Task<IActionResult> Post([FromBody]FeedbackRequestDto request)
        {
            Guid? teamId = await _dbContext.Androids.Where(x => x.Id == request.AndroidId)
                .Select(x => (Guid?)x.TeamId).SingleOrDefaultAsync();
            if (teamId.HasValue)
            {
                return Ok(new FeedbackResponseDto { TeamId = teamId.Value });
            }
            return NotFound();
        }
    }
}