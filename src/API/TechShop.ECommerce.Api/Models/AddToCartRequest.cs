namespace TechShop.ECommerce.Api.Models;

public sealed record AddToCartRequest(
    Guid ProductId,
    int Quantity
);
