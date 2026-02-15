namespace TechShop.ECommerce.Domain.Entities.Cart;

public class Cart : BaseEntity
{
    private readonly List<CartItem> _items = new();
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

    public Guid CustomerId { get; private set; }

    private Cart() { } // EF

    private Cart(Guid customerId)
    {
        CustomerId = customerId;
    }

    public static Cart Create(Guid customerId)
    {
        return new Cart(customerId);
    }

    public void AddItem(Guid productId, decimal unitPrice, int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero");

        var existingItem = _items.FirstOrDefault(x => x.ProductId == productId);

        if (existingItem is not null)
        {
            existingItem.IncreaseQuantity(quantity);
            return;
        }

        var item = new CartItem(productId, unitPrice, quantity);
        _items.Add(item);
    }

    public decimal GetTotal()
    {
        return _items.Sum(x => x.SubTotal);
    }
}
