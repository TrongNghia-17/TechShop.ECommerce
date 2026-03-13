namespace TechShop.ECommerce.Domain.Entities.Catalog;

public class Product : BaseEntity, ISoftDelete
{
    public string Name { get; private set; } = default!;
    public string? Summary { get; private set; }
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }

    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = default!;

    public string? MainImageBlobName { get; private set; }

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

        if (categoryId == Guid.Empty)
            throw new DomainException("Category is required.");

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

    public void UpdateMainImage(string blobName)
    {
        if (string.IsNullOrWhiteSpace(blobName))
            throw new DomainException("Main image blob name is required.");

        MainImageBlobName = blobName.Trim();
    }

    public void RemoveMainImage()
    {
        MainImageBlobName = null;
    }

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
            throw new DomainException(
                $"Not enough stock for product '{Name}'. Available: {StockQuantity}, Requested: {quantity}");

        StockQuantity -= quantity;
    }

    public bool HasEnoughStock(int quantity)
    {
        return StockQuantity >= quantity;
    }
}
