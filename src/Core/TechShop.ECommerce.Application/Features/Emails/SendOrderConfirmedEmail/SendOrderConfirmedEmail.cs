namespace TechShop.ECommerce.Application.Features.Emails.SendOrderConfirmedEmail;

public static class SendOrderConfirmedEmail
{
    public sealed record Command(Guid OrderId) : IRequest;

    public sealed class Handler(
        IEmailSender emailSender,
        IOrderRepository orderRepository,
        ILogger<Handler> logger
    ) : IRequestHandler<Command>
    {
        public async Task Handle(
            Command request,
            CancellationToken cancellationToken)
        {
            var order = await orderRepository
                .GetByIdAsync(request.OrderId, cancellationToken);

            if (order is null)
            {
                logger.LogWarning(
                    "Order {OrderId} not found when sending confirmation email.",
                    request.OrderId);

                return;
            }

            await emailSender.SendEmailAsync(
                new EmailMessage(
                    order.CustomerEmail,
                    $"Order {order.Id} confirmed",
                    "Thank you for your purchase!"
                ));
        }
    }
}
