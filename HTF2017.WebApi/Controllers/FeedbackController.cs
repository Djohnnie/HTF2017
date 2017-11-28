using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HTF2017.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace HTF2017.WebApi.Controllers
{
    /// <summary>
    /// These endpoints are for informational purposes and should be implemented by you on your local machine.
    /// </summary>
    /// <remarks>sjdfhsjkdfh sdfjksd fjksd fsdjk fsdjkf </remarks>
    public class FeedbackController : Controller
    {
        [HttpGet, Route("feedback")]
        public IActionResult GetFeedback()
        {
            return Ok();
        }

        [HttpPost, Route("feedback")]
        public IActionResult PostFeedback([FromBody]FeedbackRequestDto feedback)
        {
            return Ok(new FeedbackResponseDto { TeamId = Guid.NewGuid() });
        }
    }
}