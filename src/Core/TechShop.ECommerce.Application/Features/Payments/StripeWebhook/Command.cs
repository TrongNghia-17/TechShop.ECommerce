namespace TechShop.ECommerce.Application.Features.Payments.StripeWebhook;

public sealed record Command(
    string EventType,
    string SessionId,
    Guid OrderId
) : IRequest<Result>;
