namespace TechShop.ECommerce.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        // Table name
        builder.ToTable("OrderItems");

        // Composite unique constraint
        builder.HasKey(oi => new { oi.OrderId, oi.ProductId });

        // Properties
        builder.Property(oi => oi.Quantity)
            .IsRequired();

        builder.Property(oi => oi.UnitPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        // Relationships
        builder.HasOne<Order>()
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
