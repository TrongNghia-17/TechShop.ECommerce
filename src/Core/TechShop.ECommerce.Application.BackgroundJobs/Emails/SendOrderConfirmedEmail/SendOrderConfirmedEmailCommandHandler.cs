using MediatR;
using Microsoft.Extensions.Logging;
using TechShop.ECommerce.Application.Contracts.Emails;
using TechShop.ECommerce.Application.Contracts.Persistence;

namespace TechShop.ECommerce.Application.BackgroundJobs.Emails.SendOrderConfirmedEmail;

public sealed class SendOrderConfirmedEmailCommandHandler(
    IEmailSender emailSender,
    IOrderRepository orderRepository,
    IOrderConfirmationEmailBuilder emailBuilder,
    ILogger<SendOrderConfirmedEmailCommandHandler> logger)
    : IRequestHandler<SendOrderConfirmedEmailCommand>
{
    public async Task Handle(
        SendOrderConfirmedEmailCommand command,
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdWithItemsAsync(command.OrderId, cancellationToken);

        if (order is null)
        {
            logger.LogWarning(
                "Order {OrderId} not found when sending confirmation email.",
                command.OrderId);

            return;
        }

        var message = emailBuilder.Build(order);

        await emailSender.SendEmailAsync(message, cancellationToken);
    }
}