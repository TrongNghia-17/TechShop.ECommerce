using TechShop.ECommerce.Domain.Entities.Catalog;

namespace TechShop.ECommerce.Application.Features.Products.Queries.GetDetails;

public sealed class GetProductDetailsQueryHandler(
    IMapper mapper,
    IProductRepository productRepository,
    IAppLogger<GetProductDetailsQueryHandler> logger)
    : IRequestHandler<GetProductDetailsQuery, ProductDetailsDto>
{
    public async Task<ProductDetailsDto> Handle(
        GetProductDetailsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Retrieving product details for product id {ProductId}",
            request.Id);

        var product = await productRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        var data = mapper.Map<ProductDetailsDto>(product);

        logger.LogInformation(
            "Product details for product id {ProductId} retrieved successfully",
            request.Id);

        return data;
    }
}

