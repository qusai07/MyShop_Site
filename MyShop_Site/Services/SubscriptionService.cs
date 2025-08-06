
using Microsoft.EntityFrameworkCore;
using MyShop_Site.Data;
using MyShop_Site.Models;

namespace MyShop_Site.Services
{
    public class SubscriptionService
    {
        private readonly MyShopDbContext _context;

        public SubscriptionService(MyShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<SubscriptionPlan>> GetSubscriptionPlansAsync()
        {
            return await _context.SubscriptionPlans
                .OrderBy(p => p.MonthlyPrice)
                .ToListAsync();
        }

        public async Task<SubscriptionPlan?> GetSubscriptionPlanAsync(int planId)
        {
            return await _context.SubscriptionPlans
                .FirstOrDefaultAsync(p => p.Id == planId);
        }

        public async Task<List<AddOn>> GetAddOnsAsync()
        {
            return await _context.AddOns.ToListAsync();
        }

        public decimal CalculateTotal(SubscriptionPlan plan, List<AddOn> addOns, BillingCycle cycle, int userCount = 1)
        {
            var planPrice = cycle == BillingCycle.Monthly ? plan.MonthlyPrice : plan.YearlyPrice;
            var addOnPrice = addOns.Sum(a => cycle == BillingCycle.Monthly ? a.MonthlyPrice : a.YearlyPrice);
            
            return (planPrice + addOnPrice) * userCount;
        }
    }
}
