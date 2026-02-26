namespace TechShop.ECommerce.Application.Features.Carts.Commands.RemoveFromCart;

public sealed record RemoveFromCartCommand(
    Guid ProductId,
    int Quantity
) : IRequest<Result<AddToCartResult>>;
