namespace TechShop.ECommerce.Application.BackgroundJobs.Emails;

public interface IEmailJobs
{
    Task SendOrderConfirmedEmail(Guid orderId, CancellationToken token);
}