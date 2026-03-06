namespace TechShop.ECommerce.Domain.Entities.Orders;

public class Order : BaseEntity
{
    private readonly List<OrderItem> _orderItems = [];
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
    public Guid CustomerId { get; private set; }
    public string CustomerEmail { get; private set; } = default!;
    public DateTimeOffset OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }
    public string? Notes { get; private set; }
    public Address? ShippingAddress { get; private set; }
    public decimal TotalAmount { get; private set; }

    private Order() { }

    private Order(
        Guid customerId,
        string customerEmail,
        Address address,
        string? notes)
    {
        if (customerId == Guid.Empty)
            throw new DomainException("CustomerId is required.");

        if (string.IsNullOrWhiteSpace(customerEmail))
            throw new DomainException("CustomerEmail is required.");

        if (address is null)
            throw new DomainException("Shipping address is required.");

        CustomerId = customerId;
        CustomerEmail = customerEmail;

        ShippingAddress = address;
        Notes = notes;

        Status = OrderStatus.Pending;
        OrderDate = DateTimeOffset.UtcNow;
        TotalAmount = 0;
    }

    public static Order Create(
        Guid customerId,
        string customerEmail,
        Address address,
        string? notes)
    => new(customerId, customerEmail, address, notes);

    public void AddItem(
        Guid productId,
        string productName,
        decimal unitPrice,
        int quantity)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Items can only be added to a pending order.");

        if (productId == Guid.Empty)
            throw new DomainException("ProductId is required.");

        if (string.IsNullOrWhiteSpace(productName))
            throw new DomainException("ProductName is required.");

        if (unitPrice <= 0)
            throw new DomainException("UnitPrice must be greater than zero.");

        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        var existingItem = _orderItems.FirstOrDefault(i => i.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.IncreaseQuantity(quantity);
        }
        else
        {
            var newItem = new OrderItem(
                Id,
                productId,
                productName,
                unitPrice,
                quantity);

            _orderItems.Add(newItem);
        }

        CalculateTotal();
    }

    public void RemoveItem(Guid productId)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Can only remove items from a Pending order.");

        var item = _orderItems.FirstOrDefault(i => i.ProductId == productId)
            ?? throw new DomainException("Product not found in order.");

        _orderItems.Remove(item);

        CalculateTotal();
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
            throw new DomainException("Order must contain at least one item.");

        if (TotalAmount <= 0)
            throw new DomainException("TotalAmount must be greater than zero.");

        Status = OrderStatus.Confirmed;
    }
}


