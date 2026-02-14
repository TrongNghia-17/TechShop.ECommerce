namespace TechShop.ECommerce.Domain.Entities.Orders;

public class Order : BaseEntity
{
    public Guid CustomerId { get; private set; }

    public DateTimeOffset OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }
    public string? Notes { get; private set; }
    public Address? ShippingAddress { get; private set; }


    private readonly List<OrderItem> _orderItems = [];
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public decimal TotalAmount { get; private set; }

    private Order() { }

    public static Order Create(Guid customerId, Address shippingAddress, string? notes = null)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            ShippingAddress = shippingAddress,
            Notes = notes,
            Status = OrderStatus.Pending,
            OrderDate = DateTimeOffset.UtcNow,
            TotalAmount = 0
        };

        return order;
    }

    public void AddItem(Guid productId, decimal unitPrice, int quantity)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Can only add items to a Pending order.");

        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

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
    }

    private void CalculateTotal()
    {
        TotalAmount = _orderItems.Sum(i => i.UnitPrice * i.Quantity);
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
