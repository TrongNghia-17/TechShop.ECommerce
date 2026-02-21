namespace TechShop.ECommerce.Api.Models;

public sealed record RemoveFromCartRequest(
    Guid ProductId,
    int Quantity
);
