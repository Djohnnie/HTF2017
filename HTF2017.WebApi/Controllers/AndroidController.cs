using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HTF2017.Business;
using HTF2017.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace HTF2017.WebApi.Controllers
{
    public class AndroidController : Controller
    {
        private readonly AndroidLogic _androidLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidController"/> class.
        /// </summary>
        /// <param name="androidLogic">The team logic.</param>
        public AndroidController(AndroidLogic androidLogic)
        {
            _androidLogic = androidLogic;
        }

        /// <summary>
        /// Gets all deployed androids in the field.
        /// </summary>
        /// <param name="teamId">The unique identifier of the registered team to get the androids for.</param>
        /// <returns>A list of requested androids.</returns>
        /// <remarks>Gets all deployed androids in the field.</remarks>
        [HttpGet, Route("teams/{teamId}/androids")]
        public async Task<IActionResult> GetAndroids(Guid teamId)
        {
            List<AndroidDto> androids = await _androidLogic.GetAndroids(teamId);
            return Ok(androids);
        }


        /// <summary>
        /// Deploys a new android in the field.
        /// </summary>
        /// <param name="teamId">The unique identifier of the registered team to deploy an android for.</param>
        /// <param name="android">The android settings to use for deployment.</param>
        /// <returns>The deployed android.</returns>
        /// <remarks>Deploys a new android in the field using the specified deployment settings.</remarks>
        [HttpPost, Route("teams/{teamId}/androids")]
        public async Task<IActionResult> DeployAndroid(Guid teamId, DeployAndroidDto android)
        {
            AndroidDto deployedAndroid = await _androidLogic.DeployAndroid(teamId, android);
            return Ok(deployedAndroid);
        }

        /// <summary>
        /// Deploys a new android in the field.
        /// </summary>
        /// <param name="teamId">The unique identifier of the registered team that is sending this request.</param>
        /// <param name="androidId">The unique identifier of the deployed android this reuest is meant for.</param>
        /// <param name="request">The actual request.</param>
        /// <returns>The android the request is for.</returns>
        /// <remarks>Deploys a new android in the field using the specified deployment settings.</remarks>
        [HttpPost, Route("teams/{teamId}/androids/{androidId}/requests")]
        public async Task<IActionResult> SendRequest(Guid teamId, Guid androidId, AndroidRequestDto request)
        {
            AndroidDto android = await _androidLogic.SendRequest(teamId, androidId, request);
            return Ok(android);
        }
    }
}