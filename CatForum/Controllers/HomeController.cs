using System.Diagnostics;
using Azure.Core;
using CatForum.Data;
using CatForum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

// Assignment 1 final commit

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
                              .Include(d => d.Comments) // Include associated comments
                              .OrderByDescending(d => d.CreateDate)
                              .ToList();
            return View(discussions);
        }  

        // Action method with corresponding view to display a specific discussion when clicked
        public async Task<IActionResult> GetDiscussion(int? id)
        {
            var discussion = await _context.Discussion
                .Include(d => d.Comments) // Associated comments
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            return View(discussion);
        }

        // Action method for creating comment for a specific discussion post
        public IActionResult Create(int discussionId)
        {
            var comment = new Comment
            {
                DiscussionId = discussionId // Set the DiscussionId for the new comment
            };

            return View(comment);
        }

        // Action method to process comment submission, add to database, and redirect user back to GerDiscussion page
        [HttpPost]
        public IActionResult CreateComment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Comment.Add(comment);
                _context.SaveChangesAsync();
                return RedirectToAction("GetDiscussion", new { id = comment.DiscussionId });
            }

            return View(comment);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
