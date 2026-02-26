namespace TechShop.ECommerce.Domain.Entities.Cart;

public class Cart : BaseEntity
{
    private readonly List<CartItem> _items = [];
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

    public Guid CustomerId { get; private set; }

    private Cart() { } // EF

    private Cart(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new DomainException("CustomerId is required.");

        CustomerId = customerId;
    }

    public static Cart Create(Guid customerId) => new(customerId);

    public void AddItem(Guid productId, decimal unitPrice, int quantity)
    {
        if (productId == Guid.Empty)
            throw new DomainException("ProductId is required.");

        if (unitPrice <= 0)
            throw new DomainException("UnitPrice must be greater than zero.");

        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero");

        var existingItem = _items.FirstOrDefault(x => x.ProductId == productId);

        if (existingItem is not null)
        {
            existingItem.IncreaseQuantity(quantity);
            return;
        }

        _items.Add(CartItem.Create(productId, unitPrice, quantity));
    }

    public void RemoveItem(Guid productId, int quantity)
    {
        if (productId == Guid.Empty)
            throw new DomainException("ProductId is required.");

        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero");

        var existingItem = _items.FirstOrDefault(x => x.ProductId == productId)
            ?? throw new DomainException("Product does not exist in cart.");

        if (quantity >= existingItem.Quantity)
        {
            _items.Remove(existingItem);
            return;
        }

        existingItem.DecreaseQuantity(quantity);
    }

    public void Clear()
    {
        _items.Clear();
    }

    public decimal GetTotal() => _items.Sum(x => x.SubTotal);
}
