using TechShop.ECommerce.Application.Contracts.Jobs;

namespace TechShop.ECommerce.Infrastructure.Jobs.Payments;

public sealed class HangfirePaymentJobs(
    IBackgroundJobClient jobClient)
    : IPaymentJobs
{
    public Task EnqueueRefundRequiredHandling(
        Guid paymentId,
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        jobClient.Enqueue<IHangfirePaymentJobExecutor>(
            executor => executor.HandleRefundRequired(paymentId, orderId));

        return Task.CompletedTask;
    }
}
