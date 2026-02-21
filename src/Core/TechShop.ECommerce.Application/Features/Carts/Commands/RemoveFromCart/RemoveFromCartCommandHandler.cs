namespace TechShop.ECommerce.Application.Features.Carts.Commands.RemoveFromCart;

public sealed class RemoveFromCartCommandHandler(
    ICartRepository cartRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<RemoveFromCartCommand, AddToCartResult>
{
    public async Task<AddToCartResult> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
    {
        if (request.CustomerId == Guid.Empty) throw new BadRequestException("CustomerId is required.");
        if (request.ProductId == Guid.Empty) throw new BadRequestException("ProductId is required.");
        if (request.Quantity <= 0) throw new BadRequestException("Quantity must be greater than zero.");

        var cart = await cartRepository.GetByCustomerIdAsync(request.CustomerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Cart), request.CustomerId);

        cart.RemoveItem(request.ProductId, request.Quantity);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new AddToCartResult(cart.Id, cart.GetTotal());
    }
}
