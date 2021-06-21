using Eventures.App.Data;
using Eventures.App.Models;
using Eventures.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using WebApi.Models;

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
