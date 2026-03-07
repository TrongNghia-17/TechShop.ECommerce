namespace TechShop.ECommerce.Infrastructure.BackgroundJobs.Emails;

public interface IHangfireEmailJobExecutor
{
    Task SendOrderConfirmedEmail(Guid orderId);
}
