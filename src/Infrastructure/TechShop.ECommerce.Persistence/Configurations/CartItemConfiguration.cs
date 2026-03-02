namespace TechShop.ECommerce.Persistence.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        // 1. TABLE CONFIGURATION

        builder.ToTable("CartItems");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        // 2. CORE PROPERTIES

        builder.Property(c => c.ProductId)
            .IsRequired();

        builder.Property(c => c.UnitPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(c => c.Quantity)
            .IsRequired();
    }
}
