//Assignment 2 final commit 

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CatForum.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData] // property is included in download of personal data.
        public string Name { get; set; } = string.Empty; // Required

        [PersonalData]
        public string? Location { get; set; } // Optional

        [PersonalData]
        public string? ImageFilename { get; set; }// Optional

        [PersonalData]
        [NotMapped]
        public IFormFile? ImageFile { get; set; } // Optional

        // Add so users must create a custom username
        [PersonalData]
        public string DisplayName { get; set; } = string.Empty;
    }
}