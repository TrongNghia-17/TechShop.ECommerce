namespace TechShop.ECommerce.Persistence.Configurations.Orders;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // Table
        builder.ToTable("Orders");

        // Key
        builder.HasKey(order => order.Id);

        builder.Property(order => order.Id)
            .ValueGeneratedNever();

        // Properties
        builder.Property(order => order.CustomerId)
            .IsRequired();

        builder.Property(order => order.CustomerEmail)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(order => order.OrderDate)
            .IsRequired();

        builder.Property(order => order.Notes)
            .HasMaxLength(1000);

        builder.Property(order => order.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(order => order.TotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        // Indexes
        builder.HasIndex(order => order.CustomerId);
        builder.HasIndex(order => order.OrderDate);
        builder.HasIndex(order => order.Status);
        builder.HasIndex(order => new { order.CustomerId, order.OrderDate });

        // Relationships
        builder.HasMany(order => order.OrderItems)
            .WithOne()
            .HasForeignKey(orderItem => orderItem.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Owned types
        builder.OwnsOne(order => order.ShippingAddress, address =>
        {
            address.Property(value => value.Street)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("ShippingStreet");

            address.Property(value => value.City)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("ShippingCity");

            address.Property(value => value.PostalCode)
                .HasMaxLength(20)
                .HasColumnName("ShippingPostalCode");

            address.Property(value => value.Country)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("ShippingCountry");
        });
    }
}