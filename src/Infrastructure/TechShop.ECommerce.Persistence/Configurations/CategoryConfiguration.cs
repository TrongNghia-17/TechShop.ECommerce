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

        builder.Property(x => x.Id).ValueGeneratedNever();

        // Indexes
        builder.HasIndex(c => c.Name)
            .IsUnique();

        // Relationships
    }
}