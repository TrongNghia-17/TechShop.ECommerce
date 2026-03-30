using TechShop.ECommerce.Application.Common.Caching;
using TechShop.ECommerce.Application.Common.Results;
using TechShop.ECommerce.Application.Contracts.Caching;
using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Domain.Errors;

namespace TechShop.ECommerce.Application.Features.Products.UpdateProduct;

public sealed class UpdateProductCommandHandler(
    IProductRepository productRepository,
    IAppCache cache,
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
            return ProductErrors.NotFound(command.Id);

        product.Rename(command.Name);
        product.ChangePrice(command.Price);
        product.UpdateDescription(command.Summary, command.Description);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await cache.RemoveByTagAsync(CacheTags.Products, cancellationToken);

        return Result.Success();
    }
}