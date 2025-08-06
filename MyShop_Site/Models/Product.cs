
namespace MyShop_Site.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; } // Hardware, Products, Apps
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public List<string> Features { get; set; } = new List<string>();
        public string TechnicalSpecifications { get; set; }
        public List<ProductReview> Reviews { get; set; } = new List<ProductReview>();
        public string ShippingInfo { get; set; }
        public string ReturnPolicy { get; set; }
        public string SecurityCompliance { get; set; }
    }

    public class ProductReview
    {
        public string CustomerName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
