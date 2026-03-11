namespace TechShop.ECommerce.Application.Contracts.Jobs;

public interface IPaymentJobs
{
    Task EnqueueRefundRequiredHandling(
        Guid paymentId,
        Guid orderId,
        CancellationToken cancellationToken = default);
}