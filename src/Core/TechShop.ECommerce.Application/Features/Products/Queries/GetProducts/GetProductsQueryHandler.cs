namespace TechShop.ECommerce.Application.Features.Products.Queries.GetProducts;

public sealed class GetProductsQueryHandler(
    IProductRepository productRepository,
    ICacheService cache)
    : IRequestHandler<GetProductsQuery, PagedResponse<ProductDto>>
{
    public async Task<PagedResponse<ProductDto>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        var filter = new ProductQueryFilter
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            CategoryId = request.CategoryId,
            SortBy = request.SortBy,
            Search = request.Search
        };

        var shouldCache =
            filter.PageNumber <= 3 &&
            string.IsNullOrWhiteSpace(filter.Search) &&
            filter.CategoryId is null;

        if (!shouldCache)
        {
            return await productRepository
                .GetPagedAsync(filter, cancellationToken);
        }

        var version = await cache.GetOrSetAsync(
            CacheKeys.Products.VersionKey,
            () => Task.FromResult(1),
            absoluteExpiration: TimeSpan.FromHours(1)
        );

        var cacheKey = CacheKeys.Products.Paged(filter, version);

        return await cache.GetOrSetAsync(
            cacheKey,
            () => productRepository.GetPagedAsync(filter, cancellationToken),
            absoluteExpiration: TimeSpan.FromMinutes(5),
            slidingExpiration: TimeSpan.FromMinutes(2)
        ) ?? new PagedResponse<ProductDto>();
    }
}

