using TechShop.ECommerce.Domain.Abstractions;
using TechShop.ECommerce.Domain.Exceptions;

namespace TechShop.ECommerce.Domain.Entities.Orders;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; } = default!;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    private OrderItem()
    {
    }

    internal OrderItem(
        Guid orderId,
        Guid productId,
        string productName,
        decimal unitPrice,
        int quantity)
    {
        if (orderId == Guid.Empty)
        {
            throw new DomainException("Order ID is required.");
        }

        if (productId == Guid.Empty)
        {
            throw new DomainException("Product ID is required.");
        }

        if (string.IsNullOrWhiteSpace(productName))
        {
            throw new DomainException("Product name is required.");
        }

        if (unitPrice <= 0)
        {
            throw new DomainException("Unit price must be greater than zero.");
        }

        if (quantity <= 0)
        {
            throw new DomainException("Quantity must be greater than zero.");
        }

        OrderId = orderId;
        ProductId = productId;
        ProductName = productName.Trim();
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    internal void IncreaseQuantity(int amount)
    {
        if (amount <= 0)
        {
            throw new DomainException("Quantity must be greater than zero.");
        }

        Quantity += amount;
    }
}
