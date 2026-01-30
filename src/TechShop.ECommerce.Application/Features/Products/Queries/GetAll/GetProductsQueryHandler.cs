namespace TechShop.ECommerce.Application.Features.Products.Queries.GetAll;

public sealed class GetProductsQueryHandler(
    IProductRepository productRepository,
    IAppLogger<GetProductsQueryHandler> logger)
    : IRequestHandler<GetProductsQuery, IReadOnlyList<ProductDto>>
{
    public async Task<IReadOnlyList<ProductDto>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving products");

        var productDtos = await productRepository
            .GetAllAsync();

        return productDtos;
    }
}

