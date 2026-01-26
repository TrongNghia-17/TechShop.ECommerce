namespace TechShop.ECommerce.Application.Features.Product.Queries.GetAllProducts;

public class GetProductsQueryHandler(IMapper mapper,
    IProductRepository productRepository,
    IAppLogger<GetProductsQueryHandler> logger) : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetProductsWithDetailsAsync();

        var data = mapper.Map<List<ProductDto>>(products);

        logger.LogInformation("Product were retrieved successfully");
        return data;
    }
}
