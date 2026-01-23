namespace TechShop.ECommerce.Application.Features.Product.Commands.UpdateProduct;

public class UpdateProductCommandHandler(
    IMapper mapper,
    IProductRepository productRepository) : IRequestHandler<UpdateProductCommand, Unit>
{
    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var productToUpdate = mapper.Map<TechShop.ECommerce.Domain.Entities.Product>(request);

        await productRepository.UpdateAsync(productToUpdate);

        return Unit.Value;
    }
}
