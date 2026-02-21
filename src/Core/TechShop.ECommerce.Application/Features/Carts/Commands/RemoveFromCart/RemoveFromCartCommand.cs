namespace TechShop.ECommerce.Application.Features.Carts.Commands.RemoveFromCart;

public sealed record RemoveFromCartCommand(
    Guid CustomerId,
    Guid ProductId,
    int Quantity
) : IRequest<AddToCartResult>;
