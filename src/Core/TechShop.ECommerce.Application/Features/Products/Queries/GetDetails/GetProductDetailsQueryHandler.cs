namespace TechShop.ECommerce.Application.Features.Products.Queries.GetDetails;

public sealed class GetProductDetailsQueryHandler(
    IMapper mapper,
    IProductRepository productRepository,
    ICacheService cache,
    IAppLogger<GetProductDetailsQueryHandler> logger)
    : IRequestHandler<GetProductDetailsQuery, Result<ProductDetailsDto>>
{
    public async Task<Result<ProductDetailsDto>> Handle(
        GetProductDetailsQuery command,
        CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.Products.ById(command.Id);

        var cached = await cache.GetAsync<ProductDetailsDto>(cacheKey);

        if (cached is not null)
        {
            logger.LogInformation(
                "Cache hit for product {ProductId}",
                command.Id);

            return cached;
        }

        logger.LogInformation(
            "Cache miss. Retrieving product {ProductId} from DB",
            command.Id);

        var product = await productRepository
            .GetByIdAsync(command.Id, cancellationToken);

        if (product is null)
            return DomainErrors.Product.NotFound(command.Id);

        var data = mapper.Map<ProductDetailsDto>(product);

        await cache.SetAsync(
            cacheKey,
            data,
            absoluteExpiration: TimeSpan.FromMinutes(10),
            slidingExpiration: TimeSpan.FromMinutes(3));

        return data;
    }
}

