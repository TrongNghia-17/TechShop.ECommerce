namespace TechShop.ECommerce.Persistence.Configurations.Payments;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        // Table
        builder.ToTable("Payments");

        // Key
        builder.HasKey(payment => payment.Id);

        builder.Property(payment => payment.Id)
            .ValueGeneratedNever();

        // Properties
        builder.Property(payment => payment.OrderId)
            .IsRequired();

        builder.Property(payment => payment.StripeCheckoutSessionId)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(payment => payment.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(payment => payment.Currency)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(payment => payment.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        // Indexes
        builder.HasIndex(payment => payment.OrderId);

        builder.HasIndex(payment => payment.StripeCheckoutSessionId)
            .IsUnique();

        // Relationships
        builder.HasOne<Order>()
            .WithMany()
            .HasForeignKey(payment => payment.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}