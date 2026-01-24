namespace TechShop.ECommerce.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Table name
        builder.ToTable("Products");

        // Primary Key
        builder.HasKey(p => p.Id);

        // Properties
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Summary)
            .HasMaxLength(500);

        builder.Property(p => p.Description)
            .HasMaxLength(4000);

        builder.Property(p => p.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.StockQuantity)
            .IsRequired()
            .HasDefaultValue(0);

        // Indexes
        builder.HasIndex(p => p.Name);
        builder.HasIndex(p => p.CategoryId);

        // Relationships
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany<OrderItem>()
            .WithOne(oi => oi.Product)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed data
        builder.HasData(
            new Product
            {
                Id = 1,
                Name = "MacBook Pro 16 M3 Max",
                Summary = "Apple's most powerful laptop",
                Description = "16-inch Liquid Retina XDR display, M3 Max chip, 36GB Unified Memory, 1TB SSD.",
                Price = 3499.00m,
                StockQuantity = 20,
                CategoryId = 1,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            },
            new Product
            {
                Id = 2,
                Name = "Dell XPS 15",
                Summary = "Premium Windows laptop",
                Description = "15.6 FHD+ Display, Intel Core i9, NVIDIA RTX 4060, 32GB RAM, 1TB SSD.",
                Price = 2199.00m,
                StockQuantity = 15,
                CategoryId = 1,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            },
            new Product
            {
                Id = 3,
                Name = "iPhone 15 Pro Max",
                Summary = "The ultimate iPhone",
                Description = "Titanium design, A17 Pro chip, 48MP Main camera, USB-C connector.",
                Price = 1199.00m,
                StockQuantity = 50,
                CategoryId = 2,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            },
            new Product
            {
                Id = 4,
                Name = "Samsung Galaxy S24 Ultra",
                Summary = "Galaxy AI is here",
                Description = "Snapdragon 8 Gen 3, 200MP Camera, S Pen included, Titanium frame.",
                Price = 1299.00m,
                StockQuantity = 45,
                CategoryId = 2,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            },
            new Product
            {
                Id = 5,
                Name = "Logitech MX Master 3S",
                Summary = "Performance Wireless Mouse",
                Description = "8K DPI track-on-glass sensor, Quiet Clicks, USB-C rechargeable.",
                Price = 99.99m,
                StockQuantity = 100,
                CategoryId = 3,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            },
            new Product
            {
                Id = 6,
                Name = "Sony WH-1000XM5",
                Summary = "Wireless Noise Cancelling Headphones",
                Description = "Industry-leading noise canceling, 30-hour battery life, Crystal clear calling.",
                Price = 348.00m,
                StockQuantity = 30,
                CategoryId = 3,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            }
        );
    }
}
