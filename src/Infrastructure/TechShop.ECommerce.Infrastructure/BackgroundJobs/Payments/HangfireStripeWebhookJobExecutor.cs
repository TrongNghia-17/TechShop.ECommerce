using TechShop.ECommerce.Application.BackgroundJobs.Payments.ProcessCheckoutSessionCompleted;

namespace TechShop.ECommerce.Infrastructure.Jobs.Payments;

public sealed class HangfireStripeWebhookJobExecutor(
    ISender sender,
    ILogger<HangfireStripeWebhookJobExecutor> logger)
    : IHangfireStripeWebhookJobExecutor
{
    public async Task ProcessCheckoutSessionCompleted(string sessionId, Guid orderId)
    {
        logger.LogInformation(
            "Executing checkout session completed job for SessionId {SessionId}, OrderId {OrderId}",
            sessionId,
            orderId);

        await sender.Send(new ProcessCheckoutSessionCompletedCommand(sessionId, orderId));
    }
}