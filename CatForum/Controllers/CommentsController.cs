using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CatForum.Data;
using CatForum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

// Assignment 2 final commit

namespace CatForum.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly CatForumContext _context;
        // Add this
        private readonly UserManager<ApplicationUser> _userManager;
                                                           // Add this
        public CommentsController(CatForumContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            // Add this
            _userManager = userManager;
        }

        // GET: Comments/Create
        public IActionResult Create(int discussionId)
        {
            ViewData["DiscussionId"] = new SelectList(_context.Discussion, "DiscussionId", "DiscussionId");

            var comment = new Comment
            {
                DiscussionId = discussionId
            };

            return View(comment);
        }

        // POST: Comments/Create
        // This will create the new comment and redirect the user to the "Get Discussion" page if successful
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,Content,CreateDate,DiscussionId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.CreateDate = DateTime.Now; // Date/time of comment

                // Set the ApplicationUserId to the logged-in user's ID
                var currentUserId = _userManager.GetUserId(User); // Get the logged-in user's ID
                comment.ApplicationUserId = currentUserId;

                // Add the comment to the database
                _context.Add(comment);
                await _context.SaveChangesAsync();

                // Redirect user to the post they commented on
                return RedirectToAction("GetDiscussion", "Home", new { id = comment.DiscussionId });
            }

            ViewData["DiscussionId"] = new SelectList(_context.Discussion, "DiscussionId", "DiscussionId", comment.DiscussionId);
            return View(comment);
        }


        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.CommentId == id);
        }
    }
}