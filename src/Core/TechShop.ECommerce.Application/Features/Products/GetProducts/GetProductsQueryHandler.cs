using TechShop.ECommerce.Application.Common.Paging;
using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Application.Contracts.Storage;

namespace TechShop.ECommerce.Application.Features.Products.GetProducts;

public sealed class GetProductsQueryHandler(
    IProductRepository productRepository,
    IFileStorage fileStorage)
    : IRequestHandler<GetProductsQuery, PagedResponse<ProductResponse>>
{
    public async Task<PagedResponse<ProductResponse>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {

        var filter = new ProductQueryFilter
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        var pagedItems = await productRepository.GetPagedAsync(
           filter,
           cancellationToken);

        var data = pagedItems.Data
            .Select(product => new ProductResponse(
                product.Id,
                product.Name,
                product.Price,
                fileStorage.GetReadUrl(product.MainImageBlobName)))
            .ToList();

        return new PagedResponse<ProductResponse>
        {
            Data = data,
            PageNumber = pagedItems.PageNumber,
            PageSize = pagedItems.PageSize,
            TotalRecords = pagedItems.TotalRecords,
            TotalPages = pagedItems.TotalPages
        };
    }
}

