namespace TechShop.ECommerce.Application.Features.Carts.Commands.AddToCart;

public sealed record AddToCartCommand(
    Guid CustomerId,
    Guid ProductId,
    int Quantity
) : IRequest<AddToCartResult>;
