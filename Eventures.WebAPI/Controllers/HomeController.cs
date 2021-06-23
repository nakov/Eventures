using Microsoft.AspNetCore.Mvc;

namespace Eventures.WebAPI.Controllers
{
    [ApiController]
    public class HomeController : Controller
    {
        /// <summary>
        /// Gets API info (Swagger UI).
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /
        /// </remarks>
        /// <response code="200">Returns "OK" with API info.</response>
        [HttpGet]
        [Route("/")]
        [Route("api")]
        public IActionResult Index()
        {
            return LocalRedirect(@"/api/docs/index.html");
        }
    }
}
