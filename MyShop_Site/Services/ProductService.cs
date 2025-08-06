
using MyShop_Site.Models;

namespace MyShop_Site.Services
{
    public class ProductService
    {
        public List<Product> GetProductsByCategory(string category)
        {
            return GetAllProducts().Where(p => p.Category == category).ToList();
        }

        public Product GetProductById(int id)
        {
            return GetAllProducts().FirstOrDefault(p => p.Id == id);
        }

        public List<Product> GetAllProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Professional Server Hardware",
                    Description = "High-performance server solution for enterprise applications",
                    Category = "Hardware",
                    Price = 2999.99m,
                    ImageUrl = "/images/server-hardware.jpg",
                    Features = new List<string> { "64GB RAM", "2TB SSD", "Dual CPU", "24/7 Support" },
                    TechnicalSpecifications = "Intel Xeon processors, ECC memory, RAID configuration",
                    ShippingInfo = "Free shipping worldwide, 3-5 business days",
                    ReturnPolicy = "30-day money-back guarantee",
                    SecurityCompliance = "ISO 27001, SOC 2 Type II certified"
                },
                new Product
                {
                    Id = 2,
                    Name = "MyShop Analytics Pro",
                    Description = "Advanced analytics software for e-commerce businesses",
                    Category = "Products",
                    Price = 99.99m,
                    ImageUrl = "/images/analytics-software.jpg",
                    Features = new List<string> { "Real-time Analytics", "Custom Reports", "API Integration", "Multi-store Support" },
                    TechnicalSpecifications = "Cloud-based, REST API, 99.9% uptime",
                    ShippingInfo = "Digital download, instant access",
                    ReturnPolicy = "14-day trial period",
                    SecurityCompliance = "GDPR compliant, encrypted data transmission"
                },
                new Product
                {
                    Id = 3,
                    Name = "Mobile Inventory App",
                    Description = "Comprehensive inventory management mobile application",
                    Category = "Apps",
                    Price = 49.99m,
                    ImageUrl = "/images/mobile-app.jpg",
                    Features = new List<string> { "Barcode Scanning", "Real-time Sync", "Offline Mode", "Multi-location Support" },
                    TechnicalSpecifications = "iOS 14+, Android 8+, React Native",
                    ShippingInfo = "Available on App Store and Google Play",
                    ReturnPolicy = "7-day free trial",
                    SecurityCompliance = "End-to-end encryption, secure authentication"
                }
            };
        }
    }
}
