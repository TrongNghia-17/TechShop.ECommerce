namespace TechShop.ECommerce.Application.Common.Emails;

public sealed record EmailMessage(
    string To,
    string Subject,
    string HtmlBody,
    string? TextBody = null);
