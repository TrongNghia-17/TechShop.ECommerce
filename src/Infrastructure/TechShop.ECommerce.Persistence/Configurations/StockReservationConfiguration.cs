using TechShop.ECommerce.Domain.Entities.Inventory;

namespace TechShop.ECommerce.Persistence.Configurations;

public class StockReservationConfiguration
    : IEntityTypeConfiguration<StockReservation>
{
    public void Configure(EntityTypeBuilder<StockReservation> builder)
    {
        // 1. TABLE CONFIGURATION

        builder.ToTable("StockReservations");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedNever();


        // 2. CORE PROPERTIES

        builder.Property(r => r.ProductId)
            .IsRequired();

        builder.Property(r => r.OrderId)
            .IsRequired();

        builder.Property(r => r.Quantity)
            .IsRequired();

        builder.Property(r => r.ExpiresAtUtc)
            .IsRequired();


        // 3. INDEXES

        builder.HasIndex(r => r.ProductId);

        builder.HasIndex(r => r.OrderId);

        builder.HasIndex(r => r.ExpiresAtUtc);


        // 4. RELATIONSHIPS

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Order>()
            .WithMany()
            .HasForeignKey(r => r.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}