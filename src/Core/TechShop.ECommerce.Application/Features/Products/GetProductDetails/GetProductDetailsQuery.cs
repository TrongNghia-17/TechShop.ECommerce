using TechShop.ECommerce.Application.Common.Caching;
using TechShop.ECommerce.Application.Contracts.Caching;

namespace TechShop.ECommerce.Application.Features.Products.GetProductDetails;

public sealed record GetProductDetailsQuery(Guid Id)
    : IRequest<ProductDetailsDto>, ICacheable
{
    public bool BypassCache => false;
    public string CacheKey => CacheKeys.Products.ById(Id);
    public int AbsoluteExpirationInMinutes => 10;

    public IEnumerable<string> Tags => [CacheTags.Products];
}