namespace TechShop.ECommerce.Application.Contracts.Emails;

public sealed record EmailMessage(
    string To,
    string Subject,
    string HtmlBody,
    string? TextBody = null);
