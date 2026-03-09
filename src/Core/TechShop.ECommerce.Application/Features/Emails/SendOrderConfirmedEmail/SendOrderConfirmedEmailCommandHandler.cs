using TechShop.ECommerce.Application.Common.Emails;

namespace TechShop.ECommerce.Application.Features.Emails.SendOrderConfirmedEmail;

public sealed class SendOrderConfirmedEmailCommandHandler(
    IEmailSender emailSender,
    IOrderRepository orderRepository,
    IOrderConfirmationEmailBuilder emailBuilder,
    ILogger<SendOrderConfirmedEmailCommandHandler> logger)
    : IRequestHandler<SendOrderConfirmedEmailCommand>
{
    public async Task Handle(
        SendOrderConfirmedEmailCommand command,
        CancellationToken token)
    {
        var order = await orderRepository.GetByIdAsync(command.OrderId, token);

        if (order is null)
        {
            logger.LogWarning(
                "Order {OrderId} not found when sending confirmation email.",
                command.OrderId);

            return;
        }

        var message = emailBuilder.Build(order);

        await emailSender.SendEmailAsync(message, token);
    }
}