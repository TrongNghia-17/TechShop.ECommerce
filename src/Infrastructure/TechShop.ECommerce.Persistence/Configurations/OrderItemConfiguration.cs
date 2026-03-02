namespace TechShop.ECommerce.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        // 1. TABLE CONFIGURATION

        builder.ToTable("OrderItems");

        builder.HasKey(oi => new { oi.OrderId, oi.ProductId });

        builder.Property(oi => oi.Id)
            .ValueGeneratedNever();

        // 2. CORE PROPERTIES

        builder.Property(oi => oi.Quantity)
           .IsRequired();

        builder.Property(oi => oi.UnitPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        // 3. INDEXES

        builder.HasIndex(oi => oi.ProductId);

        // 4. RELATIONSHIPS

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
