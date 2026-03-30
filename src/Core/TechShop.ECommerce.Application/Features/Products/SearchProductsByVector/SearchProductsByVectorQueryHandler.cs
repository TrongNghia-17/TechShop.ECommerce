using TechShop.ECommerce.Application.Contracts.AI;
using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Application.Features.Products.Shared;

namespace TechShop.ECommerce.Application.Features.Products.SearchProductsByVector;

public sealed class SearchProductsByVectorQueryHandler(
    IEmbeddingProvider embeddingProvider,
    IPGVectorRepository vectorRepository)
    : IRequestHandler<SearchProductsByVectorQuery, IReadOnlyList<ProductSearchModel>>
{
    public async Task<IReadOnlyList<ProductSearchModel>> Handle(
        SearchProductsByVectorQuery query,
        CancellationToken cancellationToken)
    {
        var queryVector = await embeddingProvider.EmbedAsync(query.Query, cancellationToken);

        return await vectorRepository.SearchByVectorAsync(queryVector, query.TopK, cancellationToken);
    }
}
