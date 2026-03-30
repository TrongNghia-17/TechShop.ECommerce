using TechShop.ECommerce.Application.BackgroundJobs.Payments.HandleRefundRequired;

namespace TechShop.ECommerce.Infrastructure.Jobs.Payments;

public sealed class HangfirePaymentJobExecutor(
    ISender sender,
    ILogger<HangfirePaymentJobExecutor> logger)
    : IHangfirePaymentJobExecutor
{
    public async Task HandleRefundRequired(Guid paymentId, Guid orderId)
    {
        logger.LogInformation(
            "Executing refund required handling job for Payment {PaymentId}, Order {OrderId}",
            paymentId,
            orderId);

        await sender.Send(new HandleRefundRequiredCommand(paymentId, orderId));
    }
}