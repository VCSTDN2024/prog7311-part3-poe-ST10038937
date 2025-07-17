using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10038937_prog7311_poe1.Models
{
    public class Farmer
    {
        [Key]
        public int FarmerId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string FarmName { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        // Navigation property for products
        public ICollection<Product> Products { get; set; } = new List<Product>();
        
        // Foreign key to AspNetUsers table
        public string UserId { get; set; } = string.Empty;
        
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}
