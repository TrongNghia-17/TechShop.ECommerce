using TechShop.ECommerce.Domain.Abstractions;
using TechShop.ECommerce.Domain.Exceptions;

namespace TechShop.ECommerce.Domain.Entities.Catalogs;

public class Category : BaseEntity
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }

    public ICollection<Product> Products { get; private set; } = [];

    private Category()
    {
    }

    private Category(string name, string? description)
    {
        Rename(name);
        UpdateDescription(description);
    }

    public static Category Create(string name, string? description = null)
    {
        return new Category(name, description);
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Category name is required.");
        }

        Name = name.Trim();
    }

    public void UpdateDescription(string? description)
    {
        Description = NormalizeOptionalText(description);
    }

    private static string? NormalizeOptionalText(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim();
    }
}

