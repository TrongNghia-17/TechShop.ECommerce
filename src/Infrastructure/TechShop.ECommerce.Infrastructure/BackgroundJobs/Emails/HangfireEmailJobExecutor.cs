using MediatR;
using TechShop.ECommerce.Application.Features.Emails.SendOrderConfirmedEmail;

namespace TechShop.ECommerce.Infrastructure.BackgroundJobs.Emails;

public sealed class HangfireEmailJobExecutor(
    ISender sender)
    : IHangfireEmailJobExecutor
{
    public async Task SendOrderConfirmedEmail(Guid orderId)
    {
        await sender.Send(new SendOrderConfirmedEmail.Command(orderId));
    }
}
