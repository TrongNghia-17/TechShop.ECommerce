namespace TechShop.ECommerce.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Table name
        builder.ToTable("Categories");

        // Primary Key
        builder.HasKey(c => c.Id);

        // Properties
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Description)
            .HasMaxLength(1000);

        // Indexes
        builder.HasIndex(c => c.Name)
            .IsUnique();

        // Relationships
        builder.HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed data
        builder.HasData(
            new Category
            {
                Id = 1,
                Name = "Laptops & PCs",
                Description = "High-performance laptops, desktops, and workstations",
                CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false
            },
            new Category
            {
                Id = 2,
                Name = "Smartphones & Tablets",
                Description = "Latest mobile devices and tablets",
                CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false
            },
            new Category
            {
                Id = 3,
                Name = "Accessories",
                Description = "Keyboards, mice, chargers, and more",
                CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false
            }
        );
    }
}