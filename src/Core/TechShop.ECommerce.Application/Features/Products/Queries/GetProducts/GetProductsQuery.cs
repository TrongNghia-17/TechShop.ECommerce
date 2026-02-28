namespace TechShop.ECommerce.Application.Features.Products.Queries.GetProducts;

public sealed record GetProductsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    Guid? CategoryId = null,
    string? SortBy = null,
    string? Search = null
) : IRequest<PagedResponse<ProductDto>>, ICacheable
{
    public bool BypassCache =>
        PageNumber > 3 ||
        !string.IsNullOrWhiteSpace(Search) ||
        CategoryId is not null;

    public string CacheKey
    {
        get
        {
            var filter = new ProductQueryFilter
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
                CategoryId = CategoryId,
                SortBy = SortBy,
                Search = Search
            };

            return CacheKeys.Products.PagedBase(filter);
        }
    }

    public int SlidingExpirationInMinutes => 2;

    public int AbsoluteExpirationInMinutes => 5;
}

