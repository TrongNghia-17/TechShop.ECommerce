using TechShop.ECommerce.Domain.Entities.Catalogs;

namespace TechShop.ECommerce.Persistence.Configurations.Catalogs;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Table
        builder.ToTable("Products");

        // Key
        builder.HasKey(product => product.Id);

        builder.Property(product => product.Id)
            .ValueGeneratedNever();

        // Properties
        builder.Property(product => product.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(product => product.Summary)
            .HasMaxLength(500);

        builder.Property(product => product.Description)
            .HasMaxLength(4000);

        builder.Property(product => product.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(product => product.StockQuantity)
            .IsRequired();

        builder.Property(product => product.MainImageBlobName)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(product => product.Name)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        builder.HasIndex(product => product.CategoryId)
            .HasFilter("\"IsDeleted\" = false");

        builder.HasIndex(product => product.Price)
            .HasFilter("\"IsDeleted\" = false");

        // Query filters
        builder.HasQueryFilter(product => !product.IsDeleted);

        // Relationships
        builder.HasOne(product => product.Category)
            .WithMany(category => category.Products)
            .HasForeignKey(product => product.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
