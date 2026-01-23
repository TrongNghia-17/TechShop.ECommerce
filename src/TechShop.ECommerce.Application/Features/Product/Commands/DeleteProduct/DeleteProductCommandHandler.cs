namespace TechShop.ECommerce.Application.Features.Product.Commands.DeleteProduct;

public class DeleteProductCommandHandler(IProductRepository productRepository)
    : IRequestHandler<DeleteProductCommand, Unit>
{
    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var productToDelete = await productRepository.GetByIdAsync(request.Id);

        await productRepository.DeleteAsync(productToDelete);

        return Unit.Value;
    }
}
