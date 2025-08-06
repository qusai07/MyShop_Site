
using Microsoft.EntityFrameworkCore;
using MyShop_Site.Data;
using MyShop_Site.Models;

namespace MyShop_Site.Services
{
    public class OrderService
    {
        private readonly MyShopDbContext _context;

        public OrderService(MyShopDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            order.OrderNumber = GenerateOrderNumber();
            order.CreatedDate = DateTime.UtcNow;
            order.Status = OrderStatus.Pending;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<Order?> GetOrderAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.SubscriptionPlan)
                .Include(o => o.AddOns)
                    .ThenInclude(oa => oa.AddOn)
                .Include(o => o.BillingDetails)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<Order?> GetOrderByNumberAsync(string orderNumber)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.SubscriptionPlan)
                .Include(o => o.AddOns)
                    .ThenInclude(oa => oa.AddOn)
                .Include(o => o.BillingDetails)
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
        }

        public async Task<Order> CompleteOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = OrderStatus.Completed;
                await _context.SaveChangesAsync();
            }
            return order!;
        }

        private string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        }
    }
}
