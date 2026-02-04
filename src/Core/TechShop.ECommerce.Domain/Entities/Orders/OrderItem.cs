using TechShop.ECommerce.Domain.Entities.Products;

namespace TechShop.ECommerce.Domain.Entities.Orders;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
