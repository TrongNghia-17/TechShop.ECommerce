namespace TechShop.ECommerce.Application.Features.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler(
    IProductRepository productRepository,
    ProductCacheVersion productVersion,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProductCommand, Result>
{
    public async Task<Result> Handle(
        UpdateProductCommand command,
        CancellationToken cancellationToken)
    {
        var product = await productRepository
            .GetByIdAsync(command.Id, cancellationToken);

        if (product is null)
            return DomainErrors.Product.NotFound(command.Id);

        product.Rename(command.Name);
        product.ChangePrice(command.Price);
        product.UpdateDescription(command.Summary, command.Description);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await productVersion.BumpAsync(cancellationToken);

        return Result.Success();
    }
}