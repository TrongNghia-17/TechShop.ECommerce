using TechShop.ECommerce.Domain.Abstractions;
using TechShop.ECommerce.Domain.Exceptions;

namespace TechShop.ECommerce.Domain.Entities.Carts;

public class CartItem : BaseEntity
{
    public Guid CartId { get; private set; }
    public Guid ProductId { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    public decimal SubTotal => UnitPrice * Quantity;

    private CartItem()
    {
    }

    private CartItem(Guid productId, decimal unitPrice, int quantity)
    {
        if (productId == Guid.Empty)
        {
            throw new DomainException("Product ID is required.");
        }

        if (unitPrice <= 0)
        {
            throw new DomainException("Unit price must be greater than zero.");
        }

        if (quantity <= 0)
        {
            throw new DomainException("Quantity must be greater than zero.");
        }

        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    internal static CartItem Create(Guid productId, decimal unitPrice, int quantity)
    {
        return new CartItem(productId, unitPrice, quantity);
    }

    internal void IncreaseQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException("Quantity must be greater than zero.");
        }

        Quantity += quantity;
    }

    internal void DecreaseQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException("Quantity must be greater than zero.");
        }

        if (quantity > Quantity)
        {
            throw new DomainException("Quantity to remove cannot exceed current quantity.");
        }

        Quantity -= quantity;
    }

    internal void UpdateUnitPrice(decimal unitPrice)
    {
        if (unitPrice <= 0)
        {
            throw new DomainException("Unit price must be greater than zero.");
        }

        UnitPrice = unitPrice;
    }
}
