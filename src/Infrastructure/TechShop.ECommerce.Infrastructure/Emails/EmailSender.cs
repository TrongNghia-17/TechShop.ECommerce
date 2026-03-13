namespace TechShop.ECommerce.Infrastructure.Emails;

public sealed class EmailSender(
    SendGridClient client,
    IOptions<EmailOptions> options,
    ILogger<EmailSender> logger)
    : IEmailSender
{
    private readonly EmailOptions _settings = options.Value;

    public async Task SendEmailAsync(
        EmailMessage message,
        CancellationToken cancellationToken = default)
    {
        var from = new EmailAddress(_settings.FromAddress, _settings.FromName);
        var to = new EmailAddress(message.To);

        var sendGridMessage = MailHelper.CreateSingleEmail(
            from,
            to,
            message.Subject,
            message.TextBody,
            message.HtmlBody);

        var response = await client.SendEmailAsync(sendGridMessage, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Body.ReadAsStringAsync(cancellationToken);

            logger.LogError(
                "SendGrid failed: {StatusCode} {Body}",
                (int)response.StatusCode,
                body);

            throw new InvalidOperationException("SendGrid failed to send email.");
        }

        logger.LogInformation(
            "Email sent successfully to {To} with subject {Subject}",
            message.To,
            message.Subject);
    }
}
