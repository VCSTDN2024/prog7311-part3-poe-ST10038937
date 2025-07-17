using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10038937_prog7311_poe1.Models
{
    public class PostReply
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key for the user who made the reply
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }

        // Foreign key for the post this reply belongs to
        public int ForumPostId { get; set; }
        [ForeignKey("ForumPostId")]
        public virtual ForumPost? Post { get; set; }
    }
}
