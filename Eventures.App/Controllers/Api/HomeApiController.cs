using Microsoft.AspNetCore.Mvc;

namespace Eventures.App.Controllers.Api
{
    [ApiController]
    [Route("api")]
    public class HomeApiController : Controller
    {
        /// <summary>
        /// Gets API info.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /api
        /// </remarks>
        /// <response code="200">Returns "OK" with events count</response>
        [HttpGet]
        public IActionResult Index()
        {
            return LocalRedirect(@"/swagger/v1/swagger.json");
        }
    }
}
