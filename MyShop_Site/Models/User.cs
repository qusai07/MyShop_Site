
namespace MyShop_Site.Models
{
    public class User
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public CompanySize CompanySize { get; set; }
        public string Industry { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public enum CompanySize
    {
        LessThan5,
        Between5And20,
        Between20And50,
        Between50And250,
        MoreThan250
    }
}
