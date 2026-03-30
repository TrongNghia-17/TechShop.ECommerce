using TechShop.ECommerce.Domain.Entities.Carts;

namespace TechShop.ECommerce.Persistence.Configurations.Carts;

public sealed class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        // Table
        builder.ToTable("Carts");

        // Key
        builder.HasKey(cart => cart.Id);

        builder.Property(cart => cart.Id)
            .ValueGeneratedNever();

        // Properties
        builder.Property(cart => cart.CustomerId)
            .IsRequired();

        // Indexes
        builder.HasIndex(cart => cart.CustomerId)
            .IsUnique();

        // 4. RELATIONSHIPS

        // Relationships
        builder.HasMany(cart => cart.Items)
            .WithOne()
            .HasForeignKey(cartItem => cartItem.CartId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

