using Pgvector;

namespace TechShop.ECommerce.Domain.Entities.Catalogs;

public class ProductVector
{
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = default!;

    public Vector Embedding { get; private set; } = default!;

    private ProductVector() { }

    public ProductVector(Guid productId, float[] embedding)
    {
        ProductId = productId;
        Embedding = new Vector(embedding);
    }

    public void UpdateEmbedding(float[] newEmbedding)
    {
        Embedding = new Vector(newEmbedding);
    }
}
