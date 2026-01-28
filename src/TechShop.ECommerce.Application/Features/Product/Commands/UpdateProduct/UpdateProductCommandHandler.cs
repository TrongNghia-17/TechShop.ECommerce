namespace TechShop.ECommerce.Application.Features.Product.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<UpdateProductCommandHandler> logger)
    : IRequestHandler<UpdateProductCommand, Unit>
{
    public async Task<Unit> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Starting update process for Product {ProductId}",
            request.Id);

        var product = await productRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        product.Rename(request.Name);
        product.ChangePrice(request.Price);
        product.UpdateDescription(request.Summary, request.Description);

        if (product.CategoryId != request.CategoryId)
        {
            var hasOrders = await productRepository.HasOrdersAsync(product.Id);
            product.ChangeCategory(request.CategoryId, hasOrders);
        }

        await unitOfWork.SaveChangesAsync();

        logger.LogInformation(
            "Successfully updated Product {ProductId}",
            request.Id);

        return Unit.Value;
    }
}
