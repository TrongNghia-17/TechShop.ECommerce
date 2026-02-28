namespace TechShop.ECommerce.Application.Features.Products.Queries.GetProducts;

public sealed class GetProductsQueryHandler(
    ICacheVersionService versionService,
    IProductRepository productRepository)
    : IRequestHandler<GetProductsQuery, PagedResponse<ProductDto>>
{
    public async Task<PagedResponse<ProductDto>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        var version = await versionService.GetProductsVersionAsync(cancellationToken);

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

