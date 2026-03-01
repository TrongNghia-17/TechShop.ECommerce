namespace TechShop.ECommerce.Application.Features.Products.Queries.GetDetails;

public sealed record GetProductDetailsQuery(Guid Id)
    : IRequest<Result<ProductDetailsDto>>, ICacheable
{
    public bool BypassCache => false;

    public string CacheKey => CacheKeys.Products.ById(Id);

    public int SlidingExpirationInMinutes => 3;

    public int AbsoluteExpirationInMinutes => 10;
}