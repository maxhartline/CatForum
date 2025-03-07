using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CatForum.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Add this

namespace CatForum.Data
{
    // Change from DbContext to IdentityDbContext
    // Change from IdentityDbContext to IdentityDbContext<ApplicationUser>
    public class CatForumContext : IdentityDbContext<ApplicationUser>
    {
        public CatForumContext (DbContextOptions<CatForumContext> options)
            : base(options)
        {
        }

        public DbSet<CatForum.Models.Discussion> Discussion { get; set; } = default!;
        public DbSet<CatForum.Models.Comment> Comment { get; set; } = default!;
    }
}