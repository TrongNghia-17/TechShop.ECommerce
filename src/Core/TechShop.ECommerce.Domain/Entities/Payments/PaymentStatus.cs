namespace TechShop.ECommerce.Domain.Entities.Payments;

public enum PaymentStatus
{
    Pending = 0,
    Succeeded = 1,
    Failed = 2,
    Expired = 3,
    RefundPending = 4,
    Refunded = 5
}
