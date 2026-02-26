namespace TechShop.ECommerce.Application.Features.Carts.Queries.GetCart;

public sealed class GetCartQueryHandler(
    ICartRepository cartRepository,
    ICurrentUserService currentUserService
) : IRequestHandler<GetCartQuery, Result<GetCartResult>>
{
    public async Task<Result<GetCartResult>> Handle(
        GetCartQuery query,
        CancellationToken cancellationToken)
    {
        var customerId = currentUserService.UserId;

        var cart = await cartRepository
            .GetByCustomerIdAsync(customerId, cancellationToken);

        if (cart is null)
        {
            return new GetCartResult(
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

        return new GetCartResult(cart.Id, items, cart.GetTotal());
    }
}
