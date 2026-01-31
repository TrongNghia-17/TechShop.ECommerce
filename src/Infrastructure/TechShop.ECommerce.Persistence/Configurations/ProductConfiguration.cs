using TechShop.ECommerce.Domain.Entities.Orders;

namespace TechShop.ECommerce.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Table name
        builder.ToTable("Products");

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
    }
}
