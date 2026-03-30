using TechShop.ECommerce.Application.Features.Products.Shared;

namespace TechShop.ECommerce.Application.Features.Products.SearchProductsByKeyword;

public sealed record SearchProductsByKeywordQuery(string Query, int TopK = 5)
    : IRequest<IReadOnlyList<ProductSearchModel>>;
