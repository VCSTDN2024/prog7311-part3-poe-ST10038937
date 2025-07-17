using System.ComponentModel.DataAnnotations;

namespace ST10038937_prog7311_poe1.Models
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        public string? Action { get; set; } // e.g., "User Login", "Create Product", "Delete Farmer"

        public string? Details { get; set; } // e.g., "Product ID: 123, Name: Wheat"

        [Required]
        public DateTime Timestamp { get; set; }
    }
}
