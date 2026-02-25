namespace TechShop.ECommerce.Application.Features.Orders.Events.OrderPlaced;

public record OrderPlacedNotification(
    Guid OrderId,
    Guid CustomerId
) : INotification;