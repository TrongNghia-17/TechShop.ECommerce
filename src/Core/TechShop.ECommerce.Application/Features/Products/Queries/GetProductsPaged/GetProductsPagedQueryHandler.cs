namespace TechShop.ECommerce.Application.Features.Products.Queries.GetProductsPaged;

public sealed class GetProductsPagedQueryHandler(
    IProductRepository productRepository)
    : IRequestHandler<GetProductsPagedQuery, PagedResult<ProductDto>>
{
    public async Task<PagedResult<ProductDto>> Handle(
        GetProductsPagedQuery request,
        CancellationToken cancellationToken)
    {
        return await productRepository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            request.CategoryId,
            request.Sort,
            cancellationToken);
    }
}

