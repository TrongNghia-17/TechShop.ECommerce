namespace TechShop.ECommerce.Application.Features.Orders.PlaceOrder;

public sealed record PlaceOrderResponse(
    Guid OrderId,
    string CheckoutUrl);
