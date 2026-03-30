namespace TechShop.ECommerce.Infrastructure.Jobs.Emails;

public interface IHangfireEmailJobExecutor
{
    Task SendOrderConfirmedEmail(Guid orderId);
}
