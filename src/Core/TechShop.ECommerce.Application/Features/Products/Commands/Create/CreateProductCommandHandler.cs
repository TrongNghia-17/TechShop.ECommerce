namespace TechShop.ECommerce.Application.Features.Products.Commands.Create;

public sealed class CreateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<CreateProductCommandHandler> logger)
    : IRequestHandler<CreateProductCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Creating product with name {ProductName}",
            request.Name);

        var product = Product.Create(
            request.Name,
            request.Price,
            request.CategoryId,
            request.Summary,
            request.Description
        );

        await productRepository.AddAsync(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
           "Product {ProductId} created successfully",
           product.Id);

        return product.Id;
    }
}
