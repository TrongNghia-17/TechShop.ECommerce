using TechShop.ECommerce.Application.Features.Carts.Shared;

namespace TechShop.ECommerce.Application.Features.Carts.GetCart;

public sealed record GetCartResponse(
    Guid? CartId,
    IReadOnlyList<CartItemDto> Items,
    decimal Total
);
