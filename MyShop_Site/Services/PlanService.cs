
using MyShop_Site.Models;

namespace MyShop_Site.Services
{
    public class PlanService
    {
        public List<Plan> GetAllPlans()
        {
            return new List<Plan>
            {
                new Plan
                {
                    Id = 1,
                    Name = "Basic",
                    Description = "Perfect for small businesses getting started",
                    Features = new List<string> { "Up to 5 users", "Basic analytics", "Email support", "1GB storage" },
                    MonthlyPrice = 29.99m,
                    YearlyPrice = 299.99m,
                    YearlyDiscount = 0.17m
                },
                new Plan
                {
                    Id = 2,
                    Name = "Standard",
                    Description = "Great for growing businesses with advanced needs",
                    Features = new List<string> { "Up to 25 users", "Advanced analytics", "Priority support", "10GB storage", "Custom integrations" },
                    MonthlyPrice = 79.99m,
                    YearlyPrice = 799.99m,
                    YearlyDiscount = 0.17m
                },
                new Plan
                {
                    Id = 3,
                    Name = "Premium",
                    Description = "Enterprise solution with unlimited possibilities",
                    Features = new List<string> { "Unlimited users", "Enterprise analytics", "24/7 phone support", "Unlimited storage", "Custom development", "Dedicated account manager" },
                    MonthlyPrice = 199.99m,
                    YearlyPrice = 1999.99m,
                    YearlyDiscount = 0.17m
                }
            };
        }

        public List<AddOn> GetAllAddOns()
        {
            return new List<AddOn>
            {
                new AddOn
                {
                    Id = 1,
                    Name = "Advanced Security",
                    Description = "Enhanced security features including 2FA and audit logs",
                    MonthlyPrice = 19.99m,
                    YearlyPrice = 199.99m,
                    Sequence = 1
                },
                new AddOn
                {
                    Id = 2,
                    Name = "API Access",
                    Description = "Full REST API access with unlimited requests",
                    MonthlyPrice = 29.99m,
                    YearlyPrice = 299.99m,
                    Sequence = 2
                },
                new AddOn
                {
                    Id = 3,
                    Name = "Custom Branding",
                    Description = "White-label solution with your company branding",
                    MonthlyPrice = 49.99m,
                    YearlyPrice = 499.99m,
                    Sequence = 3
                }
            };
        }

        public Plan GetPlanById(int id)
        {
            return GetAllPlans().FirstOrDefault(p => p.Id == id);
        }
    }
}
