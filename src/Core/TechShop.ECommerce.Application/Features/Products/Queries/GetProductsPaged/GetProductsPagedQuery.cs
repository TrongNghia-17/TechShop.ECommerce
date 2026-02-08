using TechShop.ECommerce.Application.Common.Offset;

namespace TechShop.ECommerce.Application.Features.Products.Queries.GetProductsPaged;

public sealed record GetProductsPagedQuery(
    int PageNumber = 1,
    int PageSize = 10,
    Guid? CategoryId = null,
    string? Sort = "price"
) : IRequest<PagedResult<ProductDto>>;

