namespace TechShop.ECommerce.Api.Models;

public sealed record CartItemRequest(
    Guid ProductId,
    int Quantity
);
