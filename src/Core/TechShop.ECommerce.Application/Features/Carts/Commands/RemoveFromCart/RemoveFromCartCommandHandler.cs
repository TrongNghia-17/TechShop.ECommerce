namespace TechShop.ECommerce.Application.Features.Carts.Commands.RemoveFromCart;

public sealed class RemoveFromCartCommandHandler(
    ICartRepository cartRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
) : IRequestHandler<RemoveFromCartCommand, Result<AddToCartResult>>
{
    public async Task<Result<AddToCartResult>> Handle(
        RemoveFromCartCommand command,
        CancellationToken cancellationToken)
    {
        var customerId = currentUserService.UserId;

        var cart = await cartRepository.GetByCustomerIdAsync(
            customerId,
            cancellationToken);

        if (cart is null)
            return CartErrors.NotFound(customerId);

        try
        {
            cart.RemoveItem(command.ProductId, command.Quantity);
        }
        catch (DomainException ex)
        {
            return DomainErrors.Validation("Cart.InvalidOperation", ex.Message);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new AddToCartResult(cart.Id, cart.GetTotal());
    }
}
