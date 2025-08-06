
using System.ComponentModel.DataAnnotations;

namespace MyShop_Site.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        public string CompanyName { get; set; }
        
        [Required]
        public string ContactName { get; set; }
        
        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; }
        
        [Required]
        public string ContactPhoneNumber { get; set; }
        
        [Required]
        public string Country { get; set; }
        
        [Required]
        public string CompanySize { get; set; }
        
        public string? Industry { get; set; }
        
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
