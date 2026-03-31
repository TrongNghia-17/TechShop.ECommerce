using Stripe;
using Stripe.Checkout;
using TechShop.ECommerce.Application.Common.Configurations.PaymentGateway;

namespace TechShop.ECommerce.Infrastructure.PaymentGateway;

public sealed class StripePaymentService : IPaymentService
{
    private readonly StripeOptions _stripeOptions;

    public StripePaymentService(IOptions<StripeOptions> options)
    {
        _stripeOptions = options.Value;
        StripeConfiguration.ApiKey = _stripeOptions.SecretKey;
    }

    public async Task<CheckoutSessionResult> CreateCheckoutSessionAsync(
        Guid orderId,
        decimal amount,
        CancellationToken cancellationToken)
    {
        var currency = _stripeOptions.Currency;

        var options = new SessionCreateOptions
        {
            Mode = "payment",
            SuccessUrl = $"{_stripeOptions.SuccessUrl}?orderId={orderId}",
            CancelUrl = $"{_stripeOptions.CancelUrl}?orderId={orderId}",

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
                        UnitAmount = ConvertToMinorUnit(amount, currency),
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

    private static long ConvertToMinorUnit(decimal amount, string currency)
        => currency.ToLowerInvariant() switch
        {
            "usd" => (long)(amount * 100),
            "vnd" => (long)amount,
            _ => throw new NotSupportedException($"Unsupported currency '{currency}'.")
        };
}