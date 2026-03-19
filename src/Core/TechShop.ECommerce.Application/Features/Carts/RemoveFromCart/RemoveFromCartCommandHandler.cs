using TechShop.ECommerce.Application.Common.Results;
using TechShop.ECommerce.Application.Contracts.Identity;
using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Application.Features.Carts.Shared;
using TechShop.ECommerce.Domain.Errors;
using TechShop.ECommerce.Domain.Exceptions;

namespace TechShop.ECommerce.Application.Features.Carts.RemoveFromCart;

public sealed class RemoveFromCartCommandHandler(
    ICartRepository cartRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
) : IRequestHandler<RemoveFromCartCommand, Result<CartSummaryResponse>>
{
    public async Task<Result<CartSummaryResponse>> Handle(
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

        return new CartSummaryResponse(cart.Id, cart.GetTotal());
    }
}
