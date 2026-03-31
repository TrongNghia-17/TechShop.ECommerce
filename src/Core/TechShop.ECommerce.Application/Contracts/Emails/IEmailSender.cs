namespace TechShop.ECommerce.Application.Contracts.Emails;

public interface IEmailSender
{
    Task SendEmailAsync(
        EmailMessage message,
        CancellationToken cancellationToken = default);
}
