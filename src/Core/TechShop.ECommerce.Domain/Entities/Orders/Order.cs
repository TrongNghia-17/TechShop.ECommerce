namespace TechShop.ECommerce.Domain.Entities.Orders;

public class Order : BaseEntity
{
    private readonly List<OrderItem> _orderItems = [];
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public Guid CustomerId { get; private set; }
    public DateTimeOffset OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }
    public string? Notes { get; private set; }
    public Address? ShippingAddress { get; private set; }
    public decimal TotalAmount { get; private set; }

    private Order() { }

    private Order(Guid customerId, Address address, string? notes)
    {
        if (customerId == Guid.Empty)
            throw new DomainException("CustomerId is required.");

        CustomerId = customerId;
        ShippingAddress = address ?? throw new DomainException("Shipping address is required.");
        Notes = notes;
        Status = OrderStatus.Pending;
        OrderDate = DateTimeOffset.UtcNow;
        TotalAmount = 0;
    }

    public static Order Create(Guid customerId, Address address, string? notes)
        => new(customerId, address, notes);

    public void AddItem(Guid productId, decimal unitPrice, int quantity)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Can only add items to a Pending order.");

        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        if (unitPrice <= 0)
            throw new DomainException("UnitPrice must be greater than zero.");

        var existingItem = _orderItems.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.IncreaseQuantity(quantity);
        }
        else
        {
            var newItem = new OrderItem(Id, productId, unitPrice, quantity);
            _orderItems.Add(newItem);
        }

        CalculateTotal();
    }

    public void RemoveItem(Guid productId)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Can only remove items from a Pending order.");

        var item = _orderItems.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            _orderItems.Remove(item);
            CalculateTotal();
        }
        else
            throw new DomainException("Product not found in order.");
    }

    private void CalculateTotal()
    {
        TotalAmount = _orderItems.Sum(i => i.UnitPrice * i.Quantity);
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Order already processed.");

        if (_orderItems.Count == 0)
            throw new DomainException("Order must have at least one item.");

        Status = OrderStatus.Confirmed;
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
