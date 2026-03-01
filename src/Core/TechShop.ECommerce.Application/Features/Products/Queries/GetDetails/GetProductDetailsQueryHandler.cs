namespace TechShop.ECommerce.Application.Features.Products.Queries.GetDetails;

public sealed class GetProductDetailsQueryHandler(
    IMapper mapper,
    IProductRepository productRepository,
    IAppLogger<GetProductDetailsQueryHandler> logger)
    : IRequestHandler<GetProductDetailsQuery, Result<ProductDetailsDto>>
{
    public async Task<Result<ProductDetailsDto>> Handle(
        GetProductDetailsQuery command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Retrieving product {ProductId} from DB",
            command.Id);

        var product = await productRepository
            .GetByIdAsync(command.Id, cancellationToken);

        if (product is null)
            return DomainErrors.Product.NotFound(command.Id);

        var data = mapper.Map<ProductDetailsDto>(product);

        return data;
    }
}

