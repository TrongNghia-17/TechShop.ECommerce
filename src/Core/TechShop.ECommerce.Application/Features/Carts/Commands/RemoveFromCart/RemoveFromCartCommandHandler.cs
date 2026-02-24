namespace TechShop.ECommerce.Application.Features.Carts.Commands.RemoveFromCart;

public sealed class RemoveFromCartCommandHandler(
    ICartRepository cartRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<RemoveFromCartCommand, AddToCartResult>
{
    public async Task<AddToCartResult> Handle(
        RemoveFromCartCommand request,
        CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetByCustomerIdAsync(
            request.CustomerId,
            cancellationToken)
            ?? throw new NotFoundException(nameof(Cart), request.CustomerId);

        cart.RemoveItem(request.ProductId, request.Quantity);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new AddToCartResult(cart.Id, cart.GetTotal());
    }
}
