namespace TechShop.ECommerce.Domain.Entities.Payments;

public class Payment : BaseEntity
{
    public Guid OrderId { get; private set; }

    public string StripePaymentIntentId { get; private set; } = default!;

    public decimal Amount { get; private set; }

    public string Currency { get; private set; } = default!;

    public PaymentStatus Status { get; private set; }

    private Payment() { } // EF

    private Payment(Guid orderId, string intentId, decimal amount, string currency)
    {
        if (orderId == Guid.Empty)
            throw new DomainException("OrderId is required.");

        if (string.IsNullOrWhiteSpace(intentId))
            throw new DomainException("PaymentIntentId is required.");

        if (amount <= 0)
            throw new DomainException("Amount must be greater than zero.");

        OrderId = orderId;
        StripePaymentIntentId = intentId;
        Amount = amount;
        Currency = currency;
        Status = PaymentStatus.Pending;
    }

    public static Payment Create(
        Guid orderId,
        string intentId,
        decimal amount,
        string currency)
    => new(orderId, intentId, amount, currency);


    public void MarkSucceeded()
    {
        if (Status != PaymentStatus.Pending)
            throw new DomainException("Invalid state transition.");

        Status = PaymentStatus.Succeeded;
    }

    public void MarkFailed()
    {
        if (Status != PaymentStatus.Pending)
            throw new DomainException("Invalid state transition.");

        Status = PaymentStatus.Failed;
    }
}
