using HTF2017.Business;
using HTF2017.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HTF2017.WebApi.Controllers
{
    public class LocationsController : Controller
    {
        private readonly LocationLogic _locationLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationsController"/> class.
        /// </summary>
        /// <param name="locationLogic">The location logic.</param>
        public LocationsController(LocationLogic locationLogic)
        {
            _locationLogic = locationLogic;
        }

        /// <summary>
        /// Gets all registered locations.
        /// </summary>
        /// <returns>A list of all known locations.</returns>
        /// <remarks>Returns a list of all known locations.</remarks>
        [HttpGet, Route("locations")]
        public async Task<IActionResult> GetAllTeams()
        {
            List<LocationDto> locations = await _locationLogic.GetAllLocations();
            return Ok(locations);
        }
    }
}