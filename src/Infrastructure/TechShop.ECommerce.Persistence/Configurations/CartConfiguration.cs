namespace TechShop.ECommerce.Persistence.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        // 1. TABLE CONFIGURATION

        builder.ToTable("Carts");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        // 2. CORE PROPERTIES

        builder.Property(c => c.CustomerId)
            .IsRequired();

        // 3. INDEXES

        builder.HasIndex(c => c.CustomerId)
            .IsUnique();

        // 4. RELATIONSHIPS

        builder.HasMany(c => c.Items)
            .WithOne()
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

