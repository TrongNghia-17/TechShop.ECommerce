namespace TechShop.ECommerce.Application.Features.Products.Commands.Delete;

public sealed class DeleteProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<DeleteProductCommandHandler> logger)
    : IRequestHandler<DeleteProductCommand, Unit>
{
    public async Task<Unit> Handle(
        DeleteProductCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Deleting product with id {ProductId}",
            request.Id);

        var product = await productRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        productRepository.Delete(product);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Product {ProductId} deleted successfully",
            request.Id);

        return Unit.Value;
    }
}


