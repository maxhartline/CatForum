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
    public class DiscussionsController : Controller
    {
        private readonly CatForumContext _context;
        // Add this
        private readonly UserManager<ApplicationUser> _userManager;
                                                                // Add this
        public DiscussionsController(CatForumContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            // Add this
            _userManager = userManager;
        }

        // GET: Discussions - display all posts
        public async Task<IActionResult> Index()
        {
            // Get the current user ID
            var userId = _userManager.GetUserId(User);

            // Get only discussions created by the current user
            var discussions = await _context.Discussion
                                .Where(d => d.ApplicationUserId == userId) // Filter the Discussion objects, selecting only those where the ApplicationUserId matches the current user's Id
                                .Include(d => d.ApplicationUser) // Include the user details
                                .OrderByDescending(d => d.CreateDate)
                                .ToListAsync();

            return View(discussions);
        }

        // GET: Discussions/GetDiscussion/5 - display details of a single post
        public async Task<IActionResult> GetDiscussion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussion
                .Include(d => d.ApplicationUser)
                .Include(d => d.Comments)
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            if (discussion == null)
            {
                return NotFound();
            }

            return View("~/Views/Home/GetDiscussion.cshtml", discussion);
        }

        [Authorize]
        // GET: Discussions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Discussions/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        // Add IFormFile? ImageFile as a separate parameter so that the value from uploaded file is automatically assigned to the corresponding parameter
        public async Task<IActionResult> Create([Bind("Title,Content,ImageFilename,CreateDate,DiscussionId")] Discussion discussion, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                // Get the user ID for the currently authenticated user
                var userId = _userManager.GetUserId(User); // This assumes you are using Identity for user management.

                // If user is not logged in, redirect to login page
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                // Assign the user ID to the discussion's ApplicationUserId foreign key
                discussion.ApplicationUserId = userId;

                // Check if image file is uploaded
                if (ImageFile != null)
                {
                    // Rename the uploaded file to a GUID (unique filename) and set it before saving in database
                    discussion.ImageFilename = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);

                    // Save the uploaded file to the wwwroot/images folder
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", discussion.ImageFilename);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }
                }

                // Add the current date and time to the discussion post
                discussion.CreateDate = DateTime.Now;

                // Add the discussion to the context and save changes
                _context.Add(discussion);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index)); // Redirect to the Index action after saving
            }

            return View(discussion); // If model state is not valid, return the view with validation errors
        }



        // GET: Discussions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussion.FindAsync(id);
            if (discussion == null)
            {
                return NotFound();
            }
            return View(discussion);
        }

        // POST: Discussions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Content,ImageFilename,DiscussionId")] Discussion discussion)
        {
            if (id != discussion.DiscussionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the original post and preserve the ApplicationUserId and CreateDate
                    var originalDiscussion = await _context.Discussion
                        .AsNoTracking()
                        .FirstOrDefaultAsync(d => d.DiscussionId == discussion.DiscussionId);

                    if (originalDiscussion != null)
                    {
                        // Preserve the ApplicationUserId and CreateDate
                        discussion.ApplicationUserId = originalDiscussion.ApplicationUserId;
                        discussion.CreateDate = originalDiscussion.CreateDate;
                    }

                    // Update the discussion in the database
                    _context.Update(discussion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscussionExists(discussion.DiscussionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(discussion);
        }


        // GET: Discussions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussion
                .FirstOrDefaultAsync(m => m.DiscussionId == id);
            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        // POST: Discussions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discussion = await _context.Discussion.FindAsync(id);
            if (discussion != null)
            {
                _context.Discussion.Remove(discussion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscussionExists(int id)
        {
            return _context.Discussion.Any(e => e.DiscussionId == id);
        }
    }
}
