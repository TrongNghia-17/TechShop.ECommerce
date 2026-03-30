namespace TechShop.ECommerce.Application.Common.Emails;

public interface IEmailSender
{
    Task SendEmailAsync(
        EmailMessage message,
        CancellationToken cancellationToken = default);
}
