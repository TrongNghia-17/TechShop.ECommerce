namespace TechShop.ECommerce.Application.Features.Products.Queries.GetProductsCursor;

public sealed class GetProductsCursorHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductsCursorQuery, CursorPagedResult<ProductFeedItemDto>>
{
    public async Task<CursorPagedResult<ProductFeedItemDto>> Handle(
        GetProductsCursorQuery request,
        CancellationToken cancellationToken)
    {
        ProductCursor? cursor = null;

        if (!string.IsNullOrWhiteSpace(request.After))
            cursor = CursorEncoder.Decode<ProductCursor>(request.After);

        return await productRepository.GetAllCursorAsync(
            request.Search,
            cursor,
            request.PageSize,
            cancellationToken);
    }
}
