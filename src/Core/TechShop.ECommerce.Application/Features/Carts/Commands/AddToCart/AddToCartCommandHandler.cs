namespace TechShop.ECommerce.Application.Features.Carts.Commands.AddToCart;

public sealed class AddToCartCommandHandler(
    ICartRepository cartRepository,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<AddToCartCommand, AddToCartResult>
{
    public async Task<AddToCartResult> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        if (request.CustomerId == Guid.Empty) throw new BadRequestException("CustomerId is required.");
        if (request.ProductId == Guid.Empty) throw new BadRequestException("ProductId is required.");
        if (request.Quantity <= 0) throw new BadRequestException("Quantity must be greater than zero.");

        var product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            throw new NotFoundException(nameof(product), request.ProductId);

        var cart = await cartRepository.GetByCustomerIdAsync(request.CustomerId, cancellationToken);

        if (cart is null)
        {
            cart = Cart.Create(request.CustomerId);
            await cartRepository.AddAsync(cart, cancellationToken);
        }

        cart.AddItem(request.ProductId, product.Price, request.Quantity);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new AddToCartResult(cart.Id, cart.GetTotal());
    }
}
