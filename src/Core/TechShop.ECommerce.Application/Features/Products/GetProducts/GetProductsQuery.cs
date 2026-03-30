using TechShop.ECommerce.Application.Common.Paging;

namespace TechShop.ECommerce.Application.Features.Products.GetProducts;

public sealed record GetProductsQuery(
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PagedResponse<ProductResponse>>;
