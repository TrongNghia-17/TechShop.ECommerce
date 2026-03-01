namespace TechShop.ECommerce.Application.Features.Products.Queries.GetDetails;

public sealed class GetProductDetailsQueryHandler(
    IMapper mapper,
    IProductRepository productRepository)
    : IRequestHandler<GetProductDetailsQuery, Result<ProductDetailsDto>>
{
    public async Task<Result<ProductDetailsDto>> Handle(
        GetProductDetailsQuery command,
        CancellationToken cancellationToken)
    {
        var product = await productRepository
            .GetByIdAsync(command.Id, cancellationToken);

        if (product is null)
            return DomainErrors.Product.NotFound(command.Id);

        var data = mapper.Map<ProductDetailsDto>(product);

        return data;
    }
}

