using Microsoft.AspNetCore.Identity;

namespace ST10038937_prog7311_poe1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserRole { get; set; } // "Farmer" or "Employee"
    }
}
