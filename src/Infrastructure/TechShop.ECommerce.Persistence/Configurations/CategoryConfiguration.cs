using TechShop.ECommerce.Domain.Entities.Catalog;

namespace TechShop.ECommerce.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Table name
        builder.ToTable("Categories");

        // Properties
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Description)
            .HasMaxLength(1000);

        // Indexes
        builder.HasIndex(c => c.Name)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        // Relationships
    }
}