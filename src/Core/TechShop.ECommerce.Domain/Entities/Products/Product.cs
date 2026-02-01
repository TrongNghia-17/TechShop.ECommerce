namespace TechShop.ECommerce.Domain.Entities.Products;

public class Product : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string? Summary { get; private set; }
    public string? Description { get; private set; }
    public decimal Price { get; private set; }

    public int CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;

    private Product() { }

    public Product(
        string name,
        decimal price,
        int categoryId,
        string? summary = null,
        string? description = null)
    {
        Rename(name);
        ChangePrice(price);

        CategoryId = categoryId;
        Summary = summary;
        Description = description;
    }
    public static Product Create(
        string name,
        decimal price,
        int categoryId,
        string? summary = null,
        string? description = null)
        => new(name, price, categoryId, summary, description);
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
    public void ChangeCategory(int newCategoryId, bool hasOrders)
    {
        if (CategoryId == newCategoryId)
            return;

        // A product that already has orders must not change its category,
        // because orders depend on the original product classification.
        if (hasOrders)
            throw new DomainException(
                "Cannot change category of a product that has orders");

        CategoryId = newCategoryId;
    }

    public void Delete()
    {
        if (IsDeleted)
            throw new DomainException("Product already deleted");

        IsDeleted = true;
    }
}
