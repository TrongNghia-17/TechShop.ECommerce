using TechShop.ECommerce.Domain.ValueObjects;

namespace TechShop.ECommerce.Domain.Entities.Orders;

public class Order : BaseEntity
{
    public string UserId { get; private set; } = null!;
    public DateTimeOffset OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }

    public Address? ShippingAddress { get; private set; }
    public string? Notes { get; private set; }

    private readonly List<OrderItem> _orderItems = [];
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public decimal TotalPrice =>
        _orderItems.Sum(i => i.UnitPrice * i.Quantity);

    private Order() { }

    public Order(string userId, Address? shippingAddress = null, string? notes = null)
    {
        UserId = userId;
        ShippingAddress = shippingAddress;
        Notes = notes;
        OrderDate = DateTime.UtcNow;
        Status = OrderStatus.Pending;
    }

    public void AddItem(Guid productId, decimal unitPrice, int quantity)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Cannot modify a non-pending order");

        var existingItem = _orderItems
            .FirstOrDefault(i => i.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.IncreaseQuantity(quantity);
            return;
        }

        var orderItem = new OrderItem(Id, productId, unitPrice, quantity);
        _orderItems.Add(orderItem);
    }

    public void RemoveItem(Guid productId)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Cannot modify a non-pending order");

        var item = _orderItems.FirstOrDefault(i => i.ProductId == productId);
        if (item == null)
            return;

        _orderItems.Remove(item);
    }

    public void Confirm()
    {
        if (_orderItems.Count == 0)
            throw new DomainException("Cannot confirm an empty order");

        Status = OrderStatus.Confirmed;
    }

    public void Ship()
    {
        if (Status != OrderStatus.Confirmed)
            throw new DomainException("Order must be confirmed before shipping");

        Status = OrderStatus.Shipped;
    }

    public void Complete()
    {
        if (Status != OrderStatus.Shipped)
            throw new DomainException("Order must be shipped before completion");

        Status = OrderStatus.Completed;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Completed)
            throw new DomainException("Completed order cannot be cancelled");

        Status = OrderStatus.Cancelled;
    }

    public void UpdateShippingAddress(Address address)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Cannot change address after confirmation");

        ShippingAddress = address;
    }
}

public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Shipped = 2,
    Completed = 3,
    Cancelled = 4
}
