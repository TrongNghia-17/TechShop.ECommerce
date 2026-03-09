namespace TechShop.ECommerce.Infrastructure.BackgroundJobs.Emails;

public sealed class HangfireEmailJobExecutor(
    ISender sender)
    : IHangfireEmailJobExecutor
{
    public async Task SendOrderConfirmedEmail(Guid orderId)
    {
        await sender.Send(new SendOrderConfirmedEmailCommand(orderId));
    }
}
