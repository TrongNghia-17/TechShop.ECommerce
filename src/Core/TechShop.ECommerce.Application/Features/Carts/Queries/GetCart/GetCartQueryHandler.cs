namespace TechShop.ECommerce.Application.Features.Carts.Queries.GetCart;

public sealed class GetCartQueryHandler(
    ICartRepository cartRepository
) : IRequestHandler<GetCartQuery, GetCartResult>
{
    public async Task<GetCartResult> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        if (request.CustomerId == Guid.Empty)
            throw new BadRequestException("CustomerId is required.");

        var cart = await cartRepository.GetByCustomerIdAsync(request.CustomerId, cancellationToken);

        if (cart is null)
            return new GetCartResult(null, [], 0);

        var items = cart.Items
            .Select(item => new CartItemDto(item.ProductId, item.UnitPrice, item.Quantity, item.SubTotal))
            .ToList();

        return new GetCartResult(cart.Id, items, cart.GetTotal());
    }
}
