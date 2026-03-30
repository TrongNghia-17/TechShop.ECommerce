using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Application.Features.Products.Shared;

namespace TechShop.ECommerce.Application.Features.Products.SearchProductsByKeyword;

public sealed class SearchProductsByKeywordQueryHandler(
    IPGVectorRepository vectorRepository)
    : IRequestHandler<SearchProductsByKeywordQuery, IReadOnlyList<ProductSearchModel>>
{
    public async Task<IReadOnlyList<ProductSearchModel>> Handle(
        SearchProductsByKeywordQuery query,
        CancellationToken cancellationToken)
    {
        return await vectorRepository.SearchByKeywordAsync(query.Query, query.TopK, cancellationToken);
    }
}
