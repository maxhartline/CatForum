using CatForum.Data;

namespace CatForum.Models
{
    public class Comment
    {
        // Properties
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }

        // Foreign key
        public int DiscussionId { get; set; }

        // Navigation property - each comment belongs to one discussion - always mae nav properties nullable
        public Discussion? Discussion { get; set; } // First Discussion is the data type, second is the name of the property

        // Foreign key for ApplicationUser
        public string? ApplicationUserId { get; set; }

        // Nullable navigation property for ApplicationUser
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
