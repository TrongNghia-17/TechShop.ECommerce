namespace TechShop.ECommerce.Application.Features.Orders.Dtos;

public sealed record OrderItemDto(
    Guid ProductId,
    int Quantity
);
