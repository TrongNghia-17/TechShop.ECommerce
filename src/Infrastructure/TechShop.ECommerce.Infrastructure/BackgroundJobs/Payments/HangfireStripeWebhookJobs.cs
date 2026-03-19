using TechShop.ECommerce.Application.Contracts.Jobs;

namespace TechShop.ECommerce.Infrastructure.Jobs.Payments;

public sealed class HangfireStripeWebhookJobs(
    IBackgroundJobClient jobClient)
    : IStripeWebhookJobs
{
    public Task EnqueueCheckoutSessionCompletedProcessing(
        string sessionId,
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        jobClient.Enqueue<IHangfireStripeWebhookJobExecutor>(
            executor => executor.ProcessCheckoutSessionCompleted(sessionId, orderId));

        return Task.CompletedTask;
    }
}