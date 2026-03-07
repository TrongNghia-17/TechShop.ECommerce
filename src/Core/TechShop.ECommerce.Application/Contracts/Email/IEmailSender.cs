namespace TechShop.ECommerce.Application.Contracts.Email;

public interface IEmailSender
{
    Task<bool> SendEmailAsync(EmailMessage email);
}
