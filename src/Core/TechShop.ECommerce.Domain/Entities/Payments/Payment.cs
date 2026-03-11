namespace TechShop.ECommerce.Domain.Entities.Payments;

public class Payment : BaseEntity
{
    public Guid OrderId { get; private set; }

    public string StripeCheckoutSessionId { get; private set; } = default!;

    public decimal Amount { get; private set; }

    public string Currency { get; private set; } = default!;

    public PaymentStatus Status { get; private set; }

    private Payment() { } // EF

    private Payment(Guid orderId, string checkoutSessionId, decimal amount, string currency)
    {
        if (orderId == Guid.Empty)
            throw new DomainException("OrderId is required.");

        if (string.IsNullOrWhiteSpace(checkoutSessionId))
            throw new DomainException("CheckoutSessionId is required.");

        if (amount <= 0)
            throw new DomainException("Amount must be greater than zero.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Currency is required.");

        OrderId = orderId;
        StripeCheckoutSessionId = checkoutSessionId;
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

    public void MarkRefundPending()
    {
        if (Status == PaymentStatus.RefundPending)
            return;

        if (Status != PaymentStatus.Succeeded)
            throw new DomainException("Only succeeded payment can be marked as refund pending.");

        Status = PaymentStatus.RefundPending;
    }

    public void MarkRefunded()
    {
        if (Status == PaymentStatus.Refunded)
            return;

        if (Status != PaymentStatus.RefundPending)
            throw new DomainException("Only refund pending payment can be marked as refunded.");

        Status = PaymentStatus.Refunded;
    }

    public void Expire()
    {
        if (Status == PaymentStatus.Expired)
            return;

        if (Status != PaymentStatus.Pending)
            throw new DomainException("Only pending payment can be expired.");

        Status = PaymentStatus.Expired;
    }
}
