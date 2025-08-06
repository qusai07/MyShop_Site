
using Microsoft.EntityFrameworkCore;
using MyShop_Site.Data;
using MyShop_Site.Models;

namespace MyShop_Site.Services
{
    public class ProductService
    {
        private readonly MyShopDbContext _context;

        public ProductService(MyShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products
                .Where(p => p.IsActive)
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(ProductCategory category)
        {
            return await _context.Products
                .Where(p => p.IsActive && p.Category == category)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
        }
    }
}
