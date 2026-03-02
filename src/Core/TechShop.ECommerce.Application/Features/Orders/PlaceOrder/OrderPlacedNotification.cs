namespace TechShop.ECommerce.Application.Features.Orders.PlaceOrder;

public record OrderPlacedNotification(
    Guid OrderId,
    Guid CustomerId
) : INotification;