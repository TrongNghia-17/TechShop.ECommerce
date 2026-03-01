namespace TechShop.ECommerce.Application.Features.Products.Queries.GetProducts;

public sealed class GetProductsQueryHandler(
    IProductRepository productRepository)
    : IRequestHandler<GetProductsQuery, Result<PagedResponse<ProductDto>>>
{
    public async Task<Result<PagedResponse<ProductDto>>> Handle(
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

        var data = await productRepository.GetPagedAsync(filter, cancellationToken);

        return Result<PagedResponse<ProductDto>>.Success(data);
    }
}

