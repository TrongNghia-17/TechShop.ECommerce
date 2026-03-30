using TechShop.ECommerce.Application.Common.Results;
using TechShop.ECommerce.Application.Contracts.Identity;
using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Application.Features.Carts.Shared;

namespace TechShop.ECommerce.Application.Features.Carts.GetCart;

public sealed class GetCartQueryHandler(
    ICartRepository cartRepository,
    ICurrentUserService currentUserService
) : IRequestHandler<GetCartQuery, Result<GetCartResponse>>
{
    public async Task<Result<GetCartResponse>> Handle(
        GetCartQuery query,
        CancellationToken cancellationToken)
    {
        var customerId = currentUserService.UserId;

        var cart = await cartRepository
            .GetByCustomerIdAsync(customerId, cancellationToken);

        if (cart is null)
        {
            return new GetCartResponse(
                CartId: null,
                Items: [],
                Total: 0);
        }

        var items = cart.Items
            .Select(item => new CartItemDto(
                item.ProductId,
                item.UnitPrice,
                item.Quantity,
                item.SubTotal))
            .ToList();

        return new GetCartResponse(cart.Id, items, cart.GetTotal());
    }
}
