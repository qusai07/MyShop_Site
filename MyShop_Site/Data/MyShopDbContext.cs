
using Microsoft.EntityFrameworkCore;
using MyShop_Site.Models;

namespace MyShop_Site.Data
{
    public class MyShopDbContext : DbContext
    {
        public MyShopDbContext(DbContextOptions<MyShopDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<AddOn> AddOns { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderAddOn> OrderAddOns { get; set; }
        public DbSet<BillingDetails> BillingDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.ContactEmail)
                .IsUnique();

            // Order configuration
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.SubscriptionPlan)
                .WithMany()
                .HasForeignKey(o => o.SubscriptionPlanId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.BillingDetails)
                .WithOne()
                .HasForeignKey<Order>(o => o.Id);

            // OrderAddOn configuration
            modelBuilder.Entity<OrderAddOn>()
                .HasOne(oa => oa.AddOn)
                .WithMany()
                .HasForeignKey(oa => oa.AddOnId);

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed subscription plans
            modelBuilder.Entity<SubscriptionPlan>().HasData(
                new SubscriptionPlan
                {
                    Id = 1,
                    Name = "Basic",
                    Description = "Perfect for small businesses",
                    MonthlyPrice = 29.99m,
                    YearlyPrice = 299.99m,
                    Tier = PlanTier.Basic,
                    Features = new List<string> { "Up to 5 users", "Basic support", "Core features" }
                },
                new SubscriptionPlan
                {
                    Id = 2,
                    Name = "Standard",
                    Description = "Great for growing companies",
                    MonthlyPrice = 59.99m,
                    YearlyPrice = 599.99m,
                    Tier = PlanTier.Standard,
                    IsPopular = true,
                    Features = new List<string> { "Up to 20 users", "Priority support", "Advanced features", "API access" }
                },
                new SubscriptionPlan
                {
                    Id = 3,
                    Name = "Premium",
                    Description = "Enterprise-grade solution",
                    MonthlyPrice = 99.99m,
                    YearlyPrice = 999.99m,
                    Tier = PlanTier.Premium,
                    Features = new List<string> { "Unlimited users", "24/7 support", "All features", "Custom integrations", "Dedicated account manager" }
                }
            );

            // Seed add-ons
            modelBuilder.Entity<AddOn>().HasData(
                new AddOn
                {
                    Id = 1,
                    Name = "Extra Storage",
                    Description = "Additional 100GB storage",
                    MonthlyPrice = 10.00m,
                    YearlyPrice = 100.00m,
                    Type = AddOnType.Storage
                },
                new AddOn
                {
                    Id = 2,
                    Name = "Priority Support",
                    Description = "24/7 priority customer support",
                    MonthlyPrice = 25.00m,
                    YearlyPrice = 250.00m,
                    Type = AddOnType.Support
                }
            );

            // Seed products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Enterprise Server",
                    Description = "High-performance server solution",
                    Category = ProductCategory.Hardware,
                    Price = 2999.99m,
                    Features = "64GB RAM, 2TB SSD, Redundant Power",
                    TechnicalSpecs = "Intel Xeon, DDR4 ECC, RAID 10",
                    Rating = 4.8,
                    ReviewCount = 156
                },
                new Product
                {
                    Id = 2,
                    Name = "Business Suite",
                    Description = "Complete business management solution",
                    Category = ProductCategory.Products,
                    Price = 199.99m,
                    Features = "CRM, Inventory, Accounting, Reports",
                    TechnicalSpecs = "Cloud-based, Multi-tenant, API",
                    Rating = 4.6,
                    ReviewCount = 89
                }
            );
        }
    }
}
