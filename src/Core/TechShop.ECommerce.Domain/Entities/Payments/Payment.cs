namespace TechShop.ECommerce.Domain.Entities.Payments;

public class Payment : BaseEntity
{
    public Guid OrderId { get; private set; }
    public string StripeCheckoutSessionId { get; private set; } = default!;
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = default!;
    public PaymentStatus Status { get; private set; }

    private Payment()
    {
    }

    private Payment(
       Guid orderId,
       string checkoutSessionId,
       decimal amount,
       string currency)
    {
        if (orderId == Guid.Empty)
        {
            throw new DomainException("Order ID is required.");
        }

        if (string.IsNullOrWhiteSpace(checkoutSessionId))
        {
            throw new DomainException("Checkout session ID is required.");
        }

        if (amount <= 0)
        {
            throw new DomainException("Amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new DomainException("Currency is required.");
        }

        OrderId = orderId;
        StripeCheckoutSessionId = checkoutSessionId.Trim();
        Amount = amount;
        Currency = currency.Trim();
        Status = PaymentStatus.Pending;
    }

    public static Payment Create(
        Guid orderId,
        string checkoutSessionId,
        decimal amount,
        string currency)
    {
        return new Payment(orderId, checkoutSessionId, amount, currency);
    }


    public void MarkSucceeded()
    {
        if (Status != PaymentStatus.Pending)
        {
            throw new DomainException("Invalid state transition.");
        }

        Status = PaymentStatus.Succeeded;
    }

    public void MarkFailed()
    {
        if (Status != PaymentStatus.Pending)
        {
            throw new DomainException("Invalid state transition.");
        }

        Status = PaymentStatus.Failed;
    }

    public void MarkRefundPending()
    {
        if (Status == PaymentStatus.RefundPending)
        {
            return;
        }

        if (Status != PaymentStatus.Succeeded)
        {
            throw new DomainException("Only succeeded payments can be marked as refund pending.");
        }

        Status = PaymentStatus.RefundPending;
    }

    public void MarkRefunded()
    {
        if (Status == PaymentStatus.Refunded)
        {
            return;
        }

        if (Status != PaymentStatus.RefundPending)
        {
            throw new DomainException("Only refund pending payments can be marked as refunded.");
        }

        Status = PaymentStatus.Refunded;
    }

    public void Expire()
    {
        if (Status == PaymentStatus.Expired)
        {
            return;
        }

        if (Status != PaymentStatus.Pending)
        {
            throw new DomainException("Only pending payments can be expired.");
        }

        Status = PaymentStatus.Expired;
    }
}
