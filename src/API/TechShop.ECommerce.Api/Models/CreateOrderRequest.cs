namespace TechShop.ECommerce.Api.Models;

public sealed record CreateOrderRequest(
    AddressDto ShippingAddress,
    string? Notes
);
