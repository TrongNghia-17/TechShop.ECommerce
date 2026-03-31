namespace TechShop.ECommerce.Persistence.Configurations.Catalogs;

public class ProductVectorConfiguration : IEntityTypeConfiguration<ProductVector>
{
    public void Configure(EntityTypeBuilder<ProductVector> builder)
    {
        // Table
        builder.ToTable("ProductVectors");

        // Key
        builder.HasKey(productVector => productVector.ProductId);

        // Properties
        builder.Property(productVector => productVector.Embedding)
            .HasColumnType("vector(1536)")
            .IsRequired();

        // Indexes
        builder.HasIndex(productVector => productVector.Embedding)
            .HasMethod("hnsw")
            .HasOperators("vector_cosine_ops");

        // Relationships
        builder.HasOne(productVector => productVector.Product)
            .WithOne()
            .HasForeignKey<ProductVector>(productVector => productVector.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
