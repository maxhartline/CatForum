using System.Diagnostics;
using Azure.Core;
using CatForum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CatForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Action methods handle the HTTP request
        // 1. Receive any inputs from the request
        // 2. Perform the functionality/work to fulfill the request
        // 3. Return the View to display in the response

        // Below action method doesn't perform any work, just returns the New View
        // The system uses the View with the same name as the method (Index.cshtml)

        public IActionResult Index()
        {
            return View();
        }  

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
