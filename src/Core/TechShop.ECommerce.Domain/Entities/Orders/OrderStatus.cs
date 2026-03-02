namespace TechShop.ECommerce.Domain.Entities.Orders;

public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Shipped = 2,
    Completed = 3,
    Cancelled = 4
}
