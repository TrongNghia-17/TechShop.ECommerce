namespace TechShop.ECommerce.Application.Features.Carts.Dtos;

public sealed record CartItemDto(
    Guid ProductId,
    decimal UnitPrice,
    int Quantity,
    decimal SubTotal
);
