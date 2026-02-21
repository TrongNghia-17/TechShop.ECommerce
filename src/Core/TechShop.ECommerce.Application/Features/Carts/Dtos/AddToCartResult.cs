namespace TechShop.ECommerce.Application.Features.Carts.Dtos;

public sealed record AddToCartResult(
    Guid CartId,
    decimal Total
);
