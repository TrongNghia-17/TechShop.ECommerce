namespace TechShop.ECommerce.Domain.Entities.Inventory;

public class StockReservation : BaseEntity
{
    public Guid ProductId { get; private set; }
    public Guid OrderId { get; private set; }
    public int Quantity { get; private set; }
    public DateTimeOffset ExpiresAtUtc { get; private set; }

    public bool IsExpired => DateTimeOffset.UtcNow >= ExpiresAtUtc;

    private StockReservation() { }

    private StockReservation(
        Guid productId,
        Guid orderId,
        int quantity,
        DateTimeOffset expiresAtUtc)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        ProductId = productId;
        OrderId = orderId;
        Quantity = quantity;
        ExpiresAtUtc = expiresAtUtc;
    }

    public static StockReservation Create(
        Guid productId,
        Guid orderId,
        int quantity,
        int expireMinutes = 15)
    {
        return new StockReservation(
            productId,
            orderId,
            quantity,
            DateTimeOffset.UtcNow.AddMinutes(expireMinutes));
    }
}