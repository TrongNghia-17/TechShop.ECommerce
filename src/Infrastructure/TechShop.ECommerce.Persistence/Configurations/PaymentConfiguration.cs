namespace TechShop.ECommerce.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        // 1. TABLE CONFIGURATION

        builder.ToTable("Payments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        // 2. CORE PROPERTIES

        builder.Property(p => p.OrderId)
            .IsRequired();

        builder.Property(p => p.StripePaymentIntentId)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Currency)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        // 3. INDEXES

        builder.HasIndex(p => p.OrderId);

        builder.HasIndex(p => p.StripePaymentIntentId)
            .IsUnique();

        // 4. RELATIONSHIPS

        builder.HasOne<Order>()
            .WithMany()
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}