namespace TechShop.ECommerce.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string? Summary { get; private set; }
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }

    public int CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;

    private Product() { }

    public Product(
        string name,
        decimal price,
        int stockQuantity,
        int categoryId,
        string? summary = null,
        string? description = null)
    {
        Rename(name);
        ChangePrice(price);
        InitializeStock(stockQuantity);

        CategoryId = categoryId;
        Summary = summary;
        Description = description;
    }

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

    private void InitializeStock(int stockQuantity)
    {
        if (stockQuantity < 0)
            throw new DomainException("Stock quantity cannot be negative");

        StockQuantity = stockQuantity;
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero");

        StockQuantity += quantity;
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero");

        if (StockQuantity < quantity)
            throw new DomainException("Insufficient stock");

        StockQuantity -= quantity;
    }

    public void UpdateDescription(string? summary, string? description)
    {
        Summary = summary;
        Description = description;
    }

    public void ChangeCategory(int newCategoryId, bool hasOrders)
    {
        if (CategoryId == newCategoryId)
            return;

        if (hasOrders)
            throw new DomainException("Cannot change category of a product that has orders");

        CategoryId = newCategoryId;
    }

    public static Product Create(
        string name,
        decimal price,
        int stockQuantity,
        int categoryId,
        string? summary = null,
        string? description = null)
        => new(name, price, stockQuantity, categoryId, summary, description);
}
