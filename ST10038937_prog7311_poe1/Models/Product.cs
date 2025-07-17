using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10038937_prog7311_poe1.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;
        
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Production Date")]
        public DateTime ProductionDate { get; set; }
        
        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        [Display(Name = "Price (ZAR)")]
        public decimal Price { get; set; }
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Range(0, int.MaxValue)]
        public int QuantityAvailable { get; set; }
        
        // Foreign key to Farmer table
        public int FarmerId { get; set; }
        
        [ForeignKey("FarmerId")]
        public Farmer? Farmer { get; set; }
    }
}
