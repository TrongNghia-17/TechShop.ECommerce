using TechShop.ECommerce.Domain.Entities.Products;

namespace TechShop.ECommerce.Domain.Entities.Categories;

public class Category : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }

    public ICollection<Product> Products { get; set; } = [];
}
