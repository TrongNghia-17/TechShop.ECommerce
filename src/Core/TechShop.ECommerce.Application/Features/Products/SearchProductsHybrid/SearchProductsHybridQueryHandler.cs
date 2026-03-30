using TechShop.ECommerce.Application.Contracts.AI;
using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Application.Features.Products.Shared;

namespace TechShop.ECommerce.Application.Features.Products.SearchProductsHybrid;

public sealed class SearchProductsHybridQueryHandler(
    IEmbeddingProvider embeddingProvider,
    IPGVectorRepository vectorRepository)
    : IRequestHandler<SearchProductsHybridQuery, IReadOnlyList<ProductSearchModel>>
{
    public async Task<IReadOnlyList<ProductSearchModel>> Handle(
        SearchProductsHybridQuery query,
        CancellationToken cancellationToken)
    {
        var queryVector = await embeddingProvider.EmbedAsync(query.Query, cancellationToken);

        return await vectorRepository.HybridSearchAsync(query.Query, queryVector, query.TopK, cancellationToken);
    }
}
