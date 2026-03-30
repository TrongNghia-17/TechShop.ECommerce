using TechShop.ECommerce.Application.Contracts.Jobs;

namespace TechShop.ECommerce.Infrastructure.Jobs.Emails;

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
