namespace TechShop.ECommerce.Application.Contracts.Email;

public sealed record EmailMessage(
    string To,
    string Subject,
    string Body
);
