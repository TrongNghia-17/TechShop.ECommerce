using TechShop.ECommerce.Domain.Entities.Carts;

namespace TechShop.ECommerce.Persistence.Configurations.Carts;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        // Table
        builder.ToTable("CartItems");

        // Key
        builder.HasKey(cartItem => cartItem.Id);

        builder.Property(cartItem => cartItem.Id)
            .ValueGeneratedNever();

        // Properties
        builder.Property(cartItem => cartItem.ProductId)
            .IsRequired();

        builder.Property(cartItem => cartItem.UnitPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(cartItem => cartItem.Quantity)
            .IsRequired();
    }
}
