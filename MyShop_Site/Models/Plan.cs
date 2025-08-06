
namespace MyShop_Site.Models
{
    public class Plan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Features { get; set; } = new List<string>();
        public decimal MonthlyPrice { get; set; }
        public decimal YearlyPrice { get; set; }
        public decimal YearlyDiscount { get; set; }
    }

    public class AddOn
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MonthlyPrice { get; set; }
        public decimal YearlyPrice { get; set; }
        public int Sequence { get; set; }
    }

    public class Subscription
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PlanId { get; set; }
        public bool IsYearly { get; set; }
        public int NumberOfUsers { get; set; }
        public List<int> SelectedAddOnIds { get; set; } = new List<int>();
        public string ServerType { get; set; } // Cloud, OnPremise
        public string ServerConfiguration { get; set; } // SharedVM, SingleVM
        public string ServerUrl { get; set; }
        public string ServerCapacity { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
