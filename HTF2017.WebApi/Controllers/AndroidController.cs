using HTF2017.Business;
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
    }
}