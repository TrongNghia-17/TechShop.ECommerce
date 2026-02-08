using TechShop.ECommerce.Application.Common.Cursors;

namespace TechShop.ECommerce.Application.Features.Products.Queries.GetProductsCursor;

public sealed record GetProductsCursorQuery(
    string? Search = null,
    string? After = null,
    int PageSize = 10
) : IRequest<CursorPagedResult<ProductFeedItemDto>>;


