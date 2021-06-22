using Microsoft.AspNetCore.Mvc;

namespace Eventures.WebAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiHomeController : Controller
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
