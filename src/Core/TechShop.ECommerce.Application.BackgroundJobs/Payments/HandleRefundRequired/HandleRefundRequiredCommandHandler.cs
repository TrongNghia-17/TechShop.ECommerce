using MediatR;
using Microsoft.Extensions.Logging;
using TechShop.ECommerce.Application.Contracts.Persistence;

namespace TechShop.ECommerce.Application.BackgroundJobs.Payments.HandleRefundRequired;

public sealed class HandleRefundRequiredCommandHandler(
    IPaymentRepository paymentRepository,
    IOrderRepository orderRepository,
    ILogger<HandleRefundRequiredCommandHandler> logger)
    : IRequestHandler<HandleRefundRequiredCommand>
{
    public async Task Handle(
        HandleRefundRequiredCommand command,
        CancellationToken cancellationToken)
    {
        var payment = await paymentRepository.GetByIdAsync(
            command.PaymentId,
            cancellationToken);

        var order = await orderRepository.GetByIdWithItemsAsync(
            command.OrderId,
            cancellationToken);

        if (payment is null || order is null)
        {
            logger.LogWarning(
                "Refund handling skipped because payment {PaymentId} or order {OrderId} was not found.",
                command.PaymentId,
                command.OrderId);

            return;
        }

        logger.LogWarning(
            "Manual refund required for Payment {PaymentId}, Order {OrderId}, CustomerEmail {CustomerEmail}, Amount {Amount}.",
            payment.Id,
            order.Id,
            order.CustomerEmail,
            payment.Amount);
    }
}