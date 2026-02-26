namespace TechShop.ECommerce.Application.Features.Carts.Commands.RemoveFromCart;

public sealed class RemoveFromCartCommandHandler(
    ICartRepository cartRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
) : IRequestHandler<RemoveFromCartCommand, AddToCartResult>
{
    public async Task<AddToCartResult> Handle(
        RemoveFromCartCommand request,
        CancellationToken cancellationToken)
    {
        var customerId = currentUserService.UserId;

        var cart = await cartRepository.GetByCustomerIdAsync(
            customerId,
            cancellationToken)
            ?? throw new NotFoundException(nameof(Cart), customerId);

        cart.RemoveItem(request.ProductId, request.Quantity);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new AddToCartResult(cart.Id, cart.GetTotal());
    }
}
