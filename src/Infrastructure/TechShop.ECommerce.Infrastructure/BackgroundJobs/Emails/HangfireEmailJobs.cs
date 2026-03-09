namespace TechShop.ECommerce.Infrastructure.BackgroundJobs.Emails;

public sealed class HangfireEmailJobs(
    IBackgroundJobClient jobClient)
    : IEmailJobs
{
    public Task EnqueueOrderConfirmedEmail(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        jobClient.Enqueue<IHangfireEmailJobExecutor>(
            executor => executor.SendOrderConfirmedEmail(orderId));

        return Task.CompletedTask;
    }
}
