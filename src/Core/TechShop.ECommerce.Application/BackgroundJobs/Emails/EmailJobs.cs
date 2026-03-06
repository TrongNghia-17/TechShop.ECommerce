namespace TechShop.ECommerce.Application.BackgroundJobs.Emails;

public sealed class EmailJobs(ISender sender) : IEmailJobs
{
    public async Task SendOrderConfirmedEmail(Guid orderId, CancellationToken token)
    {
        //await sender.Send(
        //    new Features.Emails.SendOrderConfirmedEmail.Command(orderId),
        //    token);
    }
}
