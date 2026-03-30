namespace TechShop.ECommerce.Infrastructure.Emails;

public sealed class EmailSender(
    SendGridClient client,
    IOptions<EmailOptions> options,
    ILogger<EmailSender> logger)
    : IEmailSender
{
    private readonly EmailOptions _emailOptions = options.Value;

    public async Task SendEmailAsync(
        EmailMessage message,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        var from = new EmailAddress(_emailOptions.FromAddress, _emailOptions.FromName);
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
                "SendGrid failed to send email. StatusCode: {StatusCode}, Body: {Body}",
                (int)response.StatusCode,
                body);

            throw new InvalidOperationException("Failed to send email.");
        }

        logger.LogInformation(
            "Email sent successfully to {Recipient} with subject {Subject}",
            message.To,
            message.Subject);
    }
}
