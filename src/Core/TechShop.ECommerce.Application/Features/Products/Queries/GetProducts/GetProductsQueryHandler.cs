using TechShop.ECommerce.Application.Contracts.Storage;

namespace TechShop.ECommerce.Application.Features.Products.Queries.GetProducts;

public sealed class GetProductsQueryHandler(
    IProductRepository productRepository,
    IFileStorage fileStorage)
    : IRequestHandler<GetProductsQuery, PagedResponse<GetProductsResponse>>
{
    public async Task<PagedResponse<GetProductsResponse>> Handle(
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

        var pagedItems = await productRepository.GetPagedAsync(
           filter,
           cancellationToken);

        var data = pagedItems.Data
            .Select(product => new GetProductsResponse(
                product.Id,
                product.Name,
                product.Price,
                product.CategoryName,
                fileStorage.GetUrl(product.MainImageBlobName)))
            .ToList();

        return new PagedResponse<GetProductsResponse>
        {
            Data = data,
            PageNumber = pagedItems.PageNumber,
            PageSize = pagedItems.PageSize,
            TotalRecords = pagedItems.TotalRecords,
            TotalPages = pagedItems.TotalPages
        };
    }
}

