namespace TechShop.ECommerce.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // 1. TABLE CONFIGURATION

        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedNever();

        // 2. CORE PROPERTIES

        builder.Property(o => o.CustomerId)
            .IsRequired();

        builder.Property(o => o.CustomerEmail)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(o => o.OrderDate)
            .IsRequired();

        builder.Property(o => o.Notes)
            .HasMaxLength(1000);

        builder.Property(o => o.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(o => o.TotalAmount)
            .HasPrecision(18, 2);

        // 3. INDEXES

        builder.HasIndex(o => o.CustomerId);

        builder.HasIndex(o => o.OrderDate);

        builder.HasIndex(o => o.Status);

        builder.HasIndex(o => new { o.CustomerId, o.OrderDate });

        // 4. RELATIONSHIPS

        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(o => o.ShippingAddress, address =>
        {
            address.Property(a => a.Street)
                .HasMaxLength(200)
                .HasColumnName("ShippingStreet");

            address.Property(a => a.City)
                .HasMaxLength(100)
                .HasColumnName("ShippingCity");

            address.Property(a => a.PostalCode)
                .HasMaxLength(20)
                .HasColumnName("ShippingPostalCode");

            address.Property(a => a.Country)
                .HasMaxLength(100)
                .HasColumnName("ShippingCountry");
        });

    }
}