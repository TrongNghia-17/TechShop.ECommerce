namespace TechShop.ECommerce.Application.Features.Payments.StripeWebhook;

public sealed record StripeWebhookCommand(
    string EventType,
    string SessionId,
    Guid OrderId
) : IRequest<Result>;
