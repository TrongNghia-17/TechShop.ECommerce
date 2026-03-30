using TechShop.ECommerce.Application.Features.Products.Shared;

namespace TechShop.ECommerce.Application.Features.Products.SearchProductsByVector;

public sealed record SearchProductsByVectorQuery(string Query, int TopK = 5)
    : IRequest<IReadOnlyList<ProductSearchModel>>;
