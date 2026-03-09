namespace TechShop.ECommerce.Application.Features.Orders.Notifications;

public record OrderConfirmedNotification(
    Guid OrderId,
    Guid CustomerId
) : INotification;