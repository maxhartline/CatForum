using System.Diagnostics;
using Azure.Core;
using CatForum.Data;
using CatForum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CatForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CatForumContext _context; // Add DbContext

        // Add CatForumContext in the constructor
        public HomeController(ILogger<HomeController> logger, CatForumContext context)
        {
            _logger = logger;
            _context = context; // Initialize _context
        }

        // Action methods handle the HTTP request
        // 1. Receive any inputs from the request
        // 2. Perform the functionality/work to fulfill the request
        // 3. Return the View to display in the response

        // Below action method doesn't perform any work, just returns the New View
        // The system uses the View with the same name as the method (Index.cshtml)

        public IActionResult Index()
        {
            // Sort discussions by descending order on home page
            var discussions = _context.Discussion
                              .OrderByDescending(d => d.CreateDate)
                              .ToList();
            return View(discussions);
        }  

        // Create action method with corresponding view to display a specific discussion when clicked
        public async Task<IActionResult> GetDiscussion(int? id)
        {
            var discussion = await _context.Discussion
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            return View(discussion);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
