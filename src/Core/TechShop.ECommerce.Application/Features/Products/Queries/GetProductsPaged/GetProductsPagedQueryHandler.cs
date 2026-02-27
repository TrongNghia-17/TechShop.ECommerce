namespace TechShop.ECommerce.Application.Features.Products.Queries.GetProductsPaged;

public sealed class GetProductsPagedQueryHandler(
    IProductRepository productRepository)
    : IRequestHandler<GetProductsPagedQuery, PagedResponse<ProductDto>>
{
    public async Task<PagedResponse<ProductDto>> Handle(
        GetProductsPagedQuery request,
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

        return await productRepository.GetPagedAsync(filter, cancellationToken);
    }
}

