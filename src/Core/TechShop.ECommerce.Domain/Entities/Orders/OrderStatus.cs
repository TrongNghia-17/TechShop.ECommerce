namespace TechShop.ECommerce.Domain.Entities.Orders;

public enum OrderStatus
{
    PendingPayment = 0,
    Confirmed = 1,
    Expired = 2,
    Cancelled = 3
}
