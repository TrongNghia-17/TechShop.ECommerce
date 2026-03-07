using Hangfire;
using TechShop.ECommerce.Application.BackgroundJobs.Emails;

namespace TechShop.ECommerce.Infrastructure.BackgroundJobs.Emails;

public sealed class HangfireEmailJobs(
    IBackgroundJobClient backgroundJobs)
    : IEmailJobs
{
    public Task EnqueueOrderConfirmedEmail(Guid orderId, CancellationToken token)
    {
        backgroundJobs.Enqueue<IHangfireEmailJobExecutor>(
            x => x.SendOrderConfirmedEmail(orderId));

        return Task.CompletedTask;
    }
}
