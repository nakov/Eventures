using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using Eventures.Data;
using Eventures.WebApp.Models;

namespace Eventures.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        public IActionResult Index()
        {
            return View(this.dbContext.Events);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
