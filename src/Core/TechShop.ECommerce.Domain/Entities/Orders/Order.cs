using TechShop.ECommerce.Domain.Abstractions;
using TechShop.ECommerce.Domain.Exceptions;
using TechShop.ECommerce.Domain.ValueObjects;

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
    public Address ShippingAddress { get; private set; } = default!;
    public decimal TotalAmount { get; private set; }

    private Order()
    {
    }

    private Order(
        Guid customerId,
        string customerEmail,
        Address shippingAddress,
        string? notes)
    {
        if (customerId == Guid.Empty)
        {
            throw new DomainException("Customer ID is required.");
        }

        if (string.IsNullOrWhiteSpace(customerEmail))
        {
            throw new DomainException("Customer email is required.");
        }

        if (shippingAddress is null)
        {
            throw new DomainException("Shipping address is required.");
        }

        CustomerId = customerId;
        CustomerEmail = customerEmail.Trim();
        ShippingAddress = shippingAddress;
        Notes = NormalizeOptionalText(notes);
        Status = OrderStatus.PendingPayment;
        OrderDate = DateTimeOffset.UtcNow;
        TotalAmount = 0m;
    }

    public static Order Create(
        Guid customerId,
        string customerEmail,
        Address shippingAddress,
        string? notes)
    {
        return new Order(customerId, customerEmail, shippingAddress, notes);
    }

    public void AddItem(
        Guid productId,
        string productName,
        decimal unitPrice,
        int quantity)
    {
        EnsurePendingPaymentStatus();

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

        var existingItem = _orderItems.FirstOrDefault(item => item.ProductId == productId);

        if (existingItem is not null)
        {
            existingItem.IncreaseQuantity(quantity);
        }
        else
        {
            _orderItems.Add(new OrderItem(
                Id,
                productId,
                productName,
                unitPrice,
                quantity));
        }

        RecalculateTotal();
    }

    public void RemoveItem(Guid productId)
    {
        EnsurePendingPaymentStatus();

        var item = _orderItems.FirstOrDefault(item => item.ProductId == productId)
            ?? throw new DomainException("Product not found in order.");

        _orderItems.Remove(item);

        RecalculateTotal();
    }

    public void Confirm()
    {
        EnsurePendingPaymentStatus();

        if (_orderItems.Count == 0)
        {
            throw new DomainException("Order must contain at least one item.");
        }

        if (TotalAmount <= 0)
        {
            throw new DomainException("Total amount must be greater than zero.");
        }

        Status = OrderStatus.Confirmed;
    }

    public void Cancel(string? reason = null)
    {
        if (Status == OrderStatus.Cancelled)
        {
            return;
        }

        if (Status != OrderStatus.PendingPayment)
        {
            throw new DomainException("Only orders waiting for payment can be cancelled.");
        }

        Status = OrderStatus.Cancelled;

        var normalizedReason = NormalizeOptionalText(reason);

        if (normalizedReason is not null)
        {
            Notes = string.IsNullOrWhiteSpace(Notes)
                ? normalizedReason
                : $"{Notes} | {normalizedReason}";
        }
    }

    public void Expire()
    {
        EnsurePendingPaymentStatus();

        Status = OrderStatus.Expired;
    }

    private void EnsurePendingPaymentStatus()
    {
        if (Status != OrderStatus.PendingPayment)
        {
            throw new DomainException("Order is no longer waiting for payment.");
        }
    }

    private void RecalculateTotal()
    {
        TotalAmount = _orderItems.Sum(item => item.UnitPrice * item.Quantity);
    }

    private static string? NormalizeOptionalText(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim();
    }
}


