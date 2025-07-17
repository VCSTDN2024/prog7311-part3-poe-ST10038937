using System;
using System.ComponentModel.DataAnnotations;

namespace ST10038937_prog7311_poe1.Models
{
    public class ForumPost
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public virtual ApplicationUser? User { get; set; }

        public virtual ICollection<PostReply> Replies { get; set; }

        public ForumPost()
        {
            Replies = new HashSet<PostReply>();
        }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
