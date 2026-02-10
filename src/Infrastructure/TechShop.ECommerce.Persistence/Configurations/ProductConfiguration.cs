namespace TechShop.ECommerce.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Table name
        builder.ToTable("Products");

        // Properties
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

        builder.Property<NpgsqlTsVector>("SearchVector")
            .HasColumnType("tsvector")
            .HasComputedColumnSql(
                """
                to_tsvector(
                    'simple',
                    coalesce("Name", '') || ' ' ||
                    coalesce("Summary", '') || ' ' ||
                    coalesce("Description", '')
                )
                """,
                stored: true);

        builder.Property(p => p.RowVersion)
            .IsConcurrencyToken()
            .HasColumnType("integer")
            .HasDefaultValue(1)
            .ValueGeneratedOnAddOrUpdate();

        builder.HasQueryFilter("SoftDelete", p => !p.IsDeleted);

        // Indexes
        builder.HasIndex(p => p.Name)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        builder.HasIndex(p => p.CategoryId)
            .HasFilter("\"IsDeleted\" = false");

        builder.HasIndex(p => new { p.DateCreated, p.Id })
            .IsDescending(true, true)
            .HasFilter("\"IsDeleted\" = false");

        builder.HasIndex("SearchVector")
            .HasMethod("GIN")
            .HasFilter("\"IsDeleted\" = false");

        // Relationships
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany<OrderItem>()
            .WithOne()
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
