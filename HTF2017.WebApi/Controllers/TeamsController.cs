using System.Collections.Generic;
using System.Threading.Tasks;
using HTF2017.Business;
using Microsoft.AspNetCore.Mvc;
using HTF2017.DataTransferObjects;
using System;

namespace HTF2017.WebApi.Controllers
{
    public class TeamsController : Controller
    {
        private readonly TeamLogic _teamLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamsController"/> class.
        /// </summary>
        /// <param name="teamLogic">The team logic.</param>
        public TeamsController(TeamLogic teamLogic)
        {
            _teamLogic = teamLogic;
        }

        /// <summary>
        /// Gets all registered teams.
        /// </summary>
        /// <returns>A list of all registered teams.</returns>
        /// <remarks>Returns a list of all registered teams.</remarks>
        [HttpGet, Route("teams")]
        public async Task<IActionResult> GetAllTeams()
        {
            List<TeamDto> teams = await _teamLogic.GetAllTeams();
            return Ok(teams);
        }

        /// <summary>
        /// Registers a new team.
        /// </summary>
        /// <param name="team">The team to register.</param>
        /// <returns>The registered team.</returns>
        /// <remarks>Registers a new team and returns the registered team.</remarks>
        [HttpPost, Route("teams")]
        public async Task<IActionResult> RegisterTeam([FromBody]TeamRegistrationDto team)
        {
            TeamDto registeredTeam = await _teamLogic.RegisterTeam(team);
            return Ok(registeredTeam);
        }

        /// <summary>
        /// Updates a registered team.
        /// </summary>
        /// <param name="teamId">The unique identifier of the registered team to update.</param>
        /// <param name="team">The registered team to update.</param>
        /// <returns>The updated team.</returns>
        /// <remarks>Updates a registered team and returns the updated team.</remarks>
        [HttpPut, Route("teams/{teamId}")]
        public async Task<IActionResult> UpdateTeam(Guid teamId, [FromBody]TeamRegistrationDto team)
        {
            TeamDto updatedTeam = await _teamLogic.UpdateTeam(teamId, team);
            return Ok(updatedTeam);
        }

        /// <summary>
        /// Checks the status on a team feedback URL.
        /// </summary>
        /// <param name="teamId">The unique identifier of the registered team to check the feedback URL status for.</param>
        /// <returns>The feedback URL status.</returns>
        /// <remarks>Checks the status on a team feedback URL and returns the result to you.</remarks>
        [HttpGet, Route("teams/{teamId}/feedback")]
        public async Task<IActionResult> GetTeamFeedbackUrlStatus(Guid teamId)
        {
            TeamFeedbackUrlStatusDto status = await _teamLogic.GetTeamFeedbackUrlStatus(teamId);
            return Ok(status);
        }
    }
}