namespace TechShop.ECommerce.Application.Contracts.PaymentGateway;

public sealed record CheckoutSessionResult(
    string SessionId,
    string Url,
    string Currency);

public interface IPaymentService
{
    Task<CheckoutSessionResult> CreateCheckoutSessionAsync(
        Guid orderId,
        decimal amount,
        CancellationToken cancellationToken);
}
