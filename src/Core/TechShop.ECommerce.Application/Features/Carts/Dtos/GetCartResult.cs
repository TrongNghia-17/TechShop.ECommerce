namespace TechShop.ECommerce.Application.Features.Carts.Dtos;

public sealed record GetCartResult(
    Guid? CartId,
    IReadOnlyList<CartItemDto> Items,
    decimal Total
);
