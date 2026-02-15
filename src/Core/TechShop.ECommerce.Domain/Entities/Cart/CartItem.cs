namespace TechShop.ECommerce.Domain.Entities.Cart;

public class CartItem : BaseEntity
{
    public Guid CartId { get; private set; }
    public Guid ProductId { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    public decimal SubTotal => UnitPrice * Quantity;

    private CartItem() { }

    internal CartItem(Guid productId, decimal unitPrice, int quantity)
    {
        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    internal void IncreaseQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Invalid quantity");

        Quantity += quantity;
    }
}

