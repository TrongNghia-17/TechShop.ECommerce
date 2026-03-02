namespace TechShop.ECommerce.Domain.Entities.Catalog;

public class Product : BaseEntity, ISoftDelete
{
    public string Name { get; private set; } = null!;
    public string? Summary { get; private set; }
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }

    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;

    public bool IsDeleted { get; private set; }
    public DateTimeOffset? DateDeleted { get; private set; }
    public Guid? DeletedBy { get; private set; }

    private Product() { }

    private Product(
        string name,
        decimal price,
        int stockQuantity,
        Guid categoryId,
        string? summary = null,
        string? description = null)
    {
        Rename(name);
        ChangePrice(price);
        SetInitialStock(stockQuantity);

        CategoryId = categoryId;
        Summary = summary;
        Description = description;
    }
    public static Product Create(
        string name,
        decimal price,
        int stockQuantity,
        Guid categoryId,
        string? summary = null,
        string? description = null)
    => new(name, price, stockQuantity, categoryId, summary, description);
    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name is required");

        Name = name.Trim();
    }

    /// <summary>
    /// Changes the product price.
    /// Business rule:
    /// - Product price must always be greater than zero.
    /// </summary>
    public void ChangePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new DomainException("Price must be greater than zero");

        Price = newPrice;
    }

    public void UpdateDescription(string? summary, string? description)
    {
        Summary = summary;
        Description = description;
    }

    /// <summary>
    /// Changes the category of the product.
    /// Business rule:
    /// - Once a product has been ordered, its category cannot be changed
    ///   to ensure data consistency for historical orders.
    /// </summary>

    public void MarkAsDeleted(Guid? userId)
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.UtcNow;
        DeletedBy = userId;
    }

    public void Restore(Guid? userId)
    {
        IsDeleted = false;
        DateDeleted = null;
        DeletedBy = null;

        MarkAsUpdated(userId);
    }

    private void SetInitialStock(int quantity)
    {
        if (quantity < 0)
        {
            throw new DomainException("Initial stock cannot be negative.");
        }

        StockQuantity = quantity;
    }

    public void RemoveStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity to remove must be greater than zero.");

        if (StockQuantity < quantity)
            throw new DomainException($"Not enough stock for product '{Name}'. Available: {StockQuantity}, Requested: {quantity}");

        StockQuantity -= quantity;
    }

    public bool HasEnoughStock(int quantity)
    {
        return StockQuantity >= quantity;
    }
}
