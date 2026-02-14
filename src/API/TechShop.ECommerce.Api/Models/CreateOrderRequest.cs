namespace TechShop.ECommerce.Api.Models;

public sealed record CreateOrderRequest(
    List<OrderItemDto> Items,
    AddressDto ShippingAddress,
    string? Notes
);
