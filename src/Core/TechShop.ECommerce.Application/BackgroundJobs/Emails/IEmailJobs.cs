namespace TechShop.ECommerce.Application.BackgroundJobs.Emails;

public interface IEmailJobs
{
    Task EnqueueOrderConfirmedEmail(Guid orderId, CancellationToken cancellationToken = default);
}