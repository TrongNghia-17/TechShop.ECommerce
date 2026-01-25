namespace TechShop.ECommerce.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // Table name
        builder.ToTable("Orders");

        // Primary Key
        builder.HasKey(o => o.Id);

        // Properties
        builder.Property(o => o.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(o => o.OrderDate)
            .IsRequired();

        builder.Property(o => o.TotalPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(o => o.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(OrderStatus.Pending);

        builder.Property(o => o.ShippingAddress)
            .HasMaxLength(500);

        builder.Property(o => o.Notes)
            .HasMaxLength(1000);

        // Indexes
        builder.HasIndex(o => o.UserId);
        builder.HasIndex(o => o.OrderDate);
        builder.HasIndex(o => o.Status);

        // Relationships
        builder.HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed data
        builder.HasData(
            new Order
            {
                Id = 1,
                UserId = "user-001",
                OrderDate = new DateTime(2024, 1, 20, 10, 0, 0, DateTimeKind.Utc),
                TotalPrice = 3598.99m,
                Status = OrderStatus.Completed,
                ShippingAddress = "123 Tech Street, Silicon Valley, CA",
                Notes = "Please deliver during office hours",
                CreatedDate = new DateTime(2024, 1, 20, 10, 0, 0, DateTimeKind.Utc),
                IsDeleted = false
            },
            new Order
            {
                Id = 2,
                UserId = "user-002",
                OrderDate = new DateTime(2024, 1, 25, 15, 30, 0, DateTimeKind.Utc),
                TotalPrice = 1199.00m,
                Status = OrderStatus.Pending,
                ShippingAddress = "456 Developer Lane, Seattle, WA",
                Notes = "",
                CreatedDate = new DateTime(2024, 1, 25, 15, 30, 0, DateTimeKind.Utc),
                IsDeleted = false
            }
        );
    }
}