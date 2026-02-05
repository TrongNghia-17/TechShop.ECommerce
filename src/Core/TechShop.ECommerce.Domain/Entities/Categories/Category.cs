namespace TechShop.ECommerce.Domain.Entities.Categories;

public class Category : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    public ICollection<Product> Products { get; private set; } = [];

    private Category() { }

    public static Category Create(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty.", nameof(name));

        return new Category
        {
            Name = name,
            Description = description
        };
    }
}

