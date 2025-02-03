// Assignment 1 final commit

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
    }
}
