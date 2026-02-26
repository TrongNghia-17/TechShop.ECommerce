namespace TechShop.ECommerce.Application.Features.Carts.Commands.AddToCart;

public sealed class AddToCartCommandHandler(
    ICartRepository cartRepository,
    IProductRepository productRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
) : IRequestHandler<AddToCartCommand, AddToCartResult>
{
    public async Task<AddToCartResult> Handle(
        AddToCartCommand request,
        CancellationToken cancellationToken)
    {
        var customerId = currentUserService.UserId;

        var product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException(nameof(product), request.ProductId);

        var cart = await cartRepository.GetByCustomerIdAsync(customerId, cancellationToken);

        if (cart is null)
        {
            cart = Cart.Create(customerId);
            await cartRepository.AddAsync(cart, cancellationToken);
        }

        cart.AddItem(request.ProductId, product.Price, request.Quantity);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new AddToCartResult(cart.Id, cart.GetTotal());
    }
}
