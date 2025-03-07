using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CatForum.Data;

namespace CatForum.Models
{
    public class Discussion
    {
        // Properties
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ImageFilename { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }

        // One-to-many relationship between Discussion and Comment - one discussion can have many comments, each comment belongs to one discussion
        // Primary key
        public int DiscussionId { get; set; }

        // Navigation property - discussion can have many comments
        public List<Comment>? Comments { get; set; }

        // Image file upload property 
        [NotMapped]
        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }

        // Foreign key for ApplicationUser
        public string? ApplicationUserId { get; set; }

        // Nullable navigation property for ApplicationUser
        public ApplicationUser? ApplicationUser { get; set; }
    }
}