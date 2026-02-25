namespace TechShop.ECommerce.Infrastructure.EmailService;

public class EmailSender(
    IOptions<EmailSettings> options,
    ILogger<EmailSender> logger)
    : IEmailSender
{
    private readonly EmailSettings _settings = options.Value;

    public async Task<bool> SendEmail(EmailMessage email)
    {
        var client = new SendGridClient(_settings.ApiKey);

        var from = new EmailAddress(_settings.FromAddress, _settings.FromName);
        var to = new EmailAddress(email.To);

        var msg = MailHelper.CreateSingleEmail(
            from, to, email.Subject, email.Body, email.Body);

        var res = await client.SendEmailAsync(msg);

        if (!res.IsSuccessStatusCode)
        {
            var body = await res.Body.ReadAsStringAsync();
            logger.LogError("SendGrid failed: {StatusCode} {Body}", (int)res.StatusCode, body);
        }

        return res.IsSuccessStatusCode;
    }
}
