namespace TechShop.ECommerce.Application.Features.Orders.PlaceOrder;

public sealed record Response(
    Guid OrderId,
    string CheckoutUrl);
