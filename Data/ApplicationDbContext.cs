using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Twinstaranimation_backend.API.Models;

// This class represents the database context for the application.
// It is used by Entity Framework Core to interact with the database.
// It also includes ASP.NET Identity for user management.

namespace Twinstaranimation_backend.API.Data
{
    // Inherits from IdentityDbContext to include user authentication tables
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // Constructor that receives database configuration (connection string, etc.)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // Database tables (DB)
        // Each DbSet represents a table in the database
        public DbSet<Series> Series { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<ExternalLink> ExternalLinks { get; set; }

        // Model configuration using Fluent API
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Important to keep Identity configuration
            base.OnModelCreating(builder);

            // Set decimal precision for money values (avoid rounding issues)
            builder.Entity<Product>().Property(p => p.Price).HasPrecision(18, 2);

            builder.Entity<Order>().Property(o => o.TotalAmount).HasPrecision(18, 2);

            builder.Entity<OrderItem>().Property(o => o.Price).HasPrecision(18, 2);

            // Relationships
            // OrderItem has one Product (FK: ProductId) and one Order (FK: OrderId)
            // Prevent deletion of product if it is used in an order
            builder
                .Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // If order is deleted, all related order items are also deleted
            builder
                .Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
