using TechShop.ECommerce.Application.Features.Products.Shared;

namespace TechShop.ECommerce.Application.Features.Products.SearchProductsHybrid;

public sealed record SearchProductsHybridQuery(string Query, int TopK = 5)
    : IRequest<IReadOnlyList<ProductSearchModel>>;
