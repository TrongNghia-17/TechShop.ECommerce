using TechShop.ECommerce.Application.Contracts.Jobs;

namespace TechShop.ECommerce.Application.Features.Orders.Notifications;

public sealed class OrderConfirmedNotificationHandler(
    IEmailJobs emailJobs
) : INotificationHandler<OrderConfirmedNotification>
{
    public Task Handle(
        OrderConfirmedNotification notification,
        CancellationToken token)
    {
        return emailJobs.EnqueueOrderConfirmedEmail(
            notification.OrderId);
    }
}
