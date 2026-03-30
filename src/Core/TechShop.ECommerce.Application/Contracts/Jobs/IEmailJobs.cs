namespace TechShop.ECommerce.Application.Contracts.Jobs;

public interface IEmailJobs
{
    Task EnqueueOrderConfirmedEmail(Guid orderId, CancellationToken cancellationToken = default);
}