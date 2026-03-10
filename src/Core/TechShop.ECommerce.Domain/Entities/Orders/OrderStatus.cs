namespace TechShop.ECommerce.Domain.Entities.Orders;

public enum OrderStatus
{
    PendingPayment = 0,
    Confirmed = 1,
    Expired = 2,
    Shipped = 3,
    Completed = 4,
    Cancelled = 5
}
