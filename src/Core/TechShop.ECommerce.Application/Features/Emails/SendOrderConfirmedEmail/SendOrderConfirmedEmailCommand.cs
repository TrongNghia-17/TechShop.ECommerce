namespace TechShop.ECommerce.Application.Features.Emails.SendOrderConfirmedEmail;

public sealed record SendOrderConfirmedEmailCommand(Guid OrderId) : IRequest;
