namespace TechShop.ECommerce.Application.Features.Orders.Events.OrderPlaced;

public class SendOrderConfirmationEmailHandler(
    IEmailSender emailSender,
    IUserQueryService userService
) : INotificationHandler<OrderPlacedNotification>
{
    public async Task Handle(OrderPlacedNotification notification, CancellationToken token)
    {
        var user = await userService.GetCustomer(notification.CustomerId);

        await emailSender.SendEmail(new EmailMessage
        (
            To: user.Email,
            Subject: $"Order {notification.OrderId} confirmed",
            Body: "Thanks for your purchase!"
        ));
    }
}
