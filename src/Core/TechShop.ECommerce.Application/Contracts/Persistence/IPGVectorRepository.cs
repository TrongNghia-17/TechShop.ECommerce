using TechShop.ECommerce.Domain.Entities.Catalogs;
using TechShop.ECommerce.Application.Features.Products.Shared;

namespace TechShop.ECommerce.Application.Contracts.Persistence;

public interface IPGVectorRepository
{
    Task InsertProductVectorAsync(Product product, float[] embeddings, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductSearchModel>> SearchByVectorAsync(float[] queryVector, int topK = 5, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductSearchModel>> SearchByKeywordAsync(string keyword, int topK = 5, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductSearchModel>> HybridSearchAsync(string query, float[] queryVector, int topK = 5, CancellationToken cancellationToken = default);
}
