namespace TechShop.ECommerce.Application.Features.Product.Queries.GetAllProducts;

public class GetProductsQueryHandler(IMapper mapper,
    IProductRepository productRepository) : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetAsync();

        var data = mapper.Map<List<ProductDto>>(products);

        return data;
    }
}
