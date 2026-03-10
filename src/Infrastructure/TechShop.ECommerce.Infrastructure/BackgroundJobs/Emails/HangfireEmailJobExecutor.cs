using TechShop.ECommerce.Application.BackgroundJobs.Emails.SendOrderConfirmedEmail;

namespace TechShop.ECommerce.Infrastructure.BackgroundJobs.Emails;

public sealed class HangfireEmailJobExecutor(
    ISender sender,
    ILogger<HangfireEmailJobExecutor> logger)
    : IHangfireEmailJobExecutor
{
    public async Task SendOrderConfirmedEmail(Guid orderId)
    {
        logger.LogInformation(
            "Executing Hangfire email job for OrderId: {OrderId}",
            orderId);

        await sender.Send(new SendOrderConfirmedEmailCommand(orderId));
    }
}
