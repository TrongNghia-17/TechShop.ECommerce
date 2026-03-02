namespace TechShop.ECommerce.Domain.Entities.Cart;

public class CartItem : BaseEntity
{
    public Guid CartId { get; private set; }
    public Guid ProductId { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    public decimal SubTotal => UnitPrice * Quantity;

    private CartItem() { }

    private CartItem(Guid productId, decimal unitPrice, int quantity)
    {
        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    internal static CartItem Create(Guid productId, decimal unitPrice, int quantity)
        => new(productId, unitPrice, quantity);

    internal void IncreaseQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Invalid quantity");

        Quantity += quantity;
    }

    internal void DecreaseQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Invalid quantity");

        if (quantity > Quantity)
            throw new DomainException("Quantity to remove cannot exceed current quantity.");

        Quantity -= quantity;
    }

    internal void UpdateUnitPrice(decimal unitPrice)
    {
        if (unitPrice <= 0)
            throw new DomainException("Invalid unit price.");

        UnitPrice = unitPrice;
    }
}
