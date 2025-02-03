using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CatForum.Models;

// Assignment 1 final commit

namespace CatForum.Data
{
    public class CatForumContext : DbContext
    {
        public CatForumContext (DbContextOptions<CatForumContext> options)
            : base(options)
        {
        }

        public DbSet<CatForum.Models.Discussion> Discussion { get; set; } = default!;
        public DbSet<CatForum.Models.Comment> Comment { get; set; } = default!;
    }
}
