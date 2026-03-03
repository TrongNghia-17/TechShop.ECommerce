using Stripe;
using Stripe.Checkout;
using TechShop.ECommerce.Application.Contracts.PaymentGateway;

namespace TechShop.ECommerce.Infrastructure.PaymentGateway;

public sealed class StripePaymentService : IPaymentService
{
    private readonly StripeSettings _settings;

    public StripePaymentService(IOptions<StripeSettings> options)
    {
        _settings = options.Value;
        StripeConfiguration.ApiKey = _settings.SecretKey;
    }

    public async Task<CheckoutSessionResult> CreateCheckoutSessionAsync(
        Guid orderId,
        decimal amount,
        CancellationToken cancellationToken)
    {
        var currency = _settings.Currency;

        var options = new SessionCreateOptions
        {
            Mode = "payment",
            SuccessUrl = $"{_settings.SuccessUrl}?orderId={orderId}",
            CancelUrl = $"{_settings.CancelUrl}?orderId={orderId}",

            Metadata = new Dictionary<string, string>
            {
                ["orderId"] = orderId.ToString()
            },

            LineItems =
            [
                new()
                {
                    Quantity = 1,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = currency.ToLowerInvariant(),
                        UnitAmount = ConvertAmount(amount, currency),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"Order {orderId}"
                        }
                    }
                }
            ]
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options, cancellationToken: cancellationToken);

        return new CheckoutSessionResult(session.Id, session.Url, currency);
    }

    private static long ConvertAmount(decimal amount, string currency)
        => currency.ToLowerInvariant() switch
        {
            "usd" => (long)(amount * 100),
            "vnd" => (long)amount,
            _ => throw new NotSupportedException("Unsupported currency")
        };
}