namespace TechShop.ECommerce.Application.Features.Product.Queries.GetProductDetails;

public class GetProductDetailsQueryHandler(IMapper mapper,
    IProductRepository productRepository) : IRequestHandler<GetProductDetailsQuery, ProductDetailsDto>
{
    public async Task<ProductDetailsDto> Handle(GetProductDetailsQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(Product), request.Id);

        var data = mapper.Map<ProductDetailsDto>(product);

        return data;
    }
}
