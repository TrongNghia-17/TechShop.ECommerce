using TechShop.ECommerce.Domain.Entities.Catalogs;

namespace TechShop.ECommerce.Persistence.Configurations.Catalogs;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Table
        builder.ToTable("Categories");

        // Key
        builder.HasKey(category => category.Id);

        builder.Property(category => category.Id)
            .ValueGeneratedNever();

        // Properties
        builder.Property(category => category.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(category => category.Description)
            .HasMaxLength(1000);

        // Indexes
        builder.HasIndex(category => category.Name)
            .IsUnique();
    }
}