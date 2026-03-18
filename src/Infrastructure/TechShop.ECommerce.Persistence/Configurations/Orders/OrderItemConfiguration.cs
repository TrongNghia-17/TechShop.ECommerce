namespace TechShop.ECommerce.Persistence.Configurations.Orders;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        // Table
        builder.ToTable("OrderItems");

        // Key
        builder.HasKey(orderItem => orderItem.Id);

        builder.Property(orderItem => orderItem.Id)
            .ValueGeneratedNever();

        // Properties
        builder.Property(orderItem => orderItem.ProductId)
            .IsRequired();

        builder.Property(orderItem => orderItem.ProductName)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(orderItem => orderItem.Quantity)
            .IsRequired();

        builder.Property(orderItem => orderItem.UnitPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        // Indexes
        builder.HasIndex(orderItem => orderItem.OrderId);
        builder.HasIndex(orderItem => orderItem.ProductId);

        // Relationships
        builder.HasOne<Order>()
            .WithMany(order => order.OrderItems)
            .HasForeignKey(orderItem => orderItem.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
