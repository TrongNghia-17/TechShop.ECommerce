namespace TechShop.ECommerce.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // 1. TABLE CONFIGURATION

        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        // 2. CORE PROPERTIES

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Summary)
            .HasMaxLength(500);

        builder.Property(p => p.Description)
            .HasMaxLength(4000);

        builder.Property(p => p.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        // 3. INDEXES

        builder.HasIndex(p => p.Name)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        builder.HasIndex(p => p.CategoryId)
            .HasFilter("\"IsDeleted\" = false");

        builder.HasIndex(p => p.Price)
            .HasFilter("\"IsDeleted\" = false");

        // 4.QUERY FILTERS

        builder.HasQueryFilter("SoftDelete", p => !p.IsDeleted);

        // 5. RELATIONSHIPS

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
