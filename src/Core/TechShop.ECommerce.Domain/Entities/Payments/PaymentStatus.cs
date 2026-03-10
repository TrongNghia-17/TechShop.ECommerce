namespace TechShop.ECommerce.Domain.Entities.Payments;

public enum PaymentStatus
{
    Pending = 0,
    Succeeded = 1,
    Failed = 2,
    Refunded = 3,
    Expired = 4
}
