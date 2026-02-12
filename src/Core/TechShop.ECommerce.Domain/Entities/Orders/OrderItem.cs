namespace TechShop.ECommerce.Domain.Entities.Orders;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    private OrderItem() { }

    internal OrderItem(
        Guid orderId,
        Guid productId,
        decimal unitPrice,
        int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero");

        OrderId = orderId;
        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    internal void IncreaseQuantity(int amount)
    {
        if (amount <= 0)
            throw new DomainException("Quantity must be positive");

        Quantity += amount;
    }
}
