namespace TechShop.ECommerce.Application.Features.Product.Commands.UpdateProduct;

public class UpdateProductCommandHandler(
    IMapper mapper,
    IProductRepository productRepository,
    IAppLogger<UpdateProductCommandHandler> logger) : IRequestHandler<UpdateProductCommand, Unit>
{
    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateProductCommandValidator(productRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count != 0)
        {
            logger.LogWarning("Validation errors in update request for {0} - {1}", nameof(Product), request.Id);
            throw new BadRequestException("Invalid Product", validationResult);
        }

        var productToUpdate = mapper.Map<TechShop.ECommerce.Domain.Entities.Product>(request);

        await productRepository.UpdateAsync(productToUpdate);

        return Unit.Value;
    }
}
