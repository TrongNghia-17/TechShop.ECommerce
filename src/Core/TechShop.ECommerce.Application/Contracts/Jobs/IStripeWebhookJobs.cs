namespace TechShop.ECommerce.Application.Contracts.Jobs;

public interface IStripeWebhookJobs
{
    Task EnqueueCheckoutSessionCompletedProcessing(
        string sessionId,
        Guid orderId,
        CancellationToken cancellationToken = default);
}
