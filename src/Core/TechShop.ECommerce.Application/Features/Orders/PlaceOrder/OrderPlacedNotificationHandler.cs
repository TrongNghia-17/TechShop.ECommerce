using TechShop.ECommerce.Application.BackgroundJobs.Emails;

namespace TechShop.ECommerce.Application.Features.Orders.PlaceOrder;

public class OrderPlacedNotificationHandler(
    IEmailJobs emailJobs
) : INotificationHandler<OrderPlacedNotification>
{
    public async Task Handle(
        OrderPlacedNotification notification,
        CancellationToken token)
    {
        await emailJobs.EnqueueOrderConfirmedEmail(
            notification.OrderId,
            token);
    }
}
