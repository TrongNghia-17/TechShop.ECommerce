using TechShop.ECommerce.Application.Features.Products.Shared;
using TechShop.ECommerce.Domain.Entities.Catalogs;

namespace TechShop.ECommerce.Application.Contracts.Persistence;

public interface IPGVectorRepository
{
    Task InsertProductVectorAsync(Product product, float[] embeddings, CancellationToken cancellationToken = default);

    Task UpsertProductVectorsAsync(IEnumerable<(Product Product, float[] Embedding)> batch, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductSearchModel>> SearchByVectorAsync(float[] queryVector, int topK = 5, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductSearchModel>> HybridSearchAsync(string query, float[] queryVector, int topK = 5, CancellationToken cancellationToken = default);
}
