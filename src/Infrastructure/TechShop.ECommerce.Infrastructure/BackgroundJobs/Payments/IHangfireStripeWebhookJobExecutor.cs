namespace TechShop.ECommerce.Infrastructure.Jobs.Payments;

public interface IHangfireStripeWebhookJobExecutor
{
    Task ProcessCheckoutSessionCompleted(string sessionId, Guid orderId);
}
