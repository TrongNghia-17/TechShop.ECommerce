namespace TechShop.ECommerce.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        // Table name
        builder.ToTable("OrderItems");

        // Primary Key
        builder.HasKey(oi => oi.Id);

        // Composite unique constraint
        builder.HasIndex(oi => new { oi.OrderId, oi.ProductId })
            .IsUnique();

        // Properties
        builder.Property(oi => oi.Quantity)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(oi => oi.UnitPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        // Relationships
        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(oi => oi.Product)
            .WithMany()
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed data
        builder.HasData(
            new OrderItem
            {
                Id = 1,
                OrderId = 1,
                ProductId = 1,
                Quantity = 1,
                UnitPrice = 3499.00m,
                CreatedDate = DateTime.UtcNow.AddDays(-10),
                IsDeleted = false
            },
            new OrderItem
            {
                Id = 2,
                OrderId = 1,
                ProductId = 5,
                Quantity = 1,
                UnitPrice = 99.99m,
                CreatedDate = DateTime.UtcNow.AddDays(-10),
                IsDeleted = false
            },
            new OrderItem
            {
                Id = 3,
                OrderId = 2,
                ProductId = 3,
                Quantity = 1,
                UnitPrice = 1199.00m,
                CreatedDate = DateTime.UtcNow.AddHours(-2),
                IsDeleted = false
            }
        );
    }
}
