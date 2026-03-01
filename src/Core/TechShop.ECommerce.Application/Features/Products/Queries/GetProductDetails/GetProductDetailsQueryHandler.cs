namespace TechShop.ECommerce.Application.Features.Products.Queries.GetProductDetails;

public sealed class GetProductDetailsQueryHandler(
    IMapper mapper,
    IProductRepository productRepository)
    : IRequestHandler<GetProductDetailsQuery, ProductDetailsDto>
{
    public async Task<ProductDetailsDto> Handle(
        GetProductDetailsQuery query,
        CancellationToken cancellationToken)
    {
        var product = await productRepository
            .GetByIdAsync(query.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), query.Id);

        return mapper.Map<ProductDetailsDto>(product);
    }
}

