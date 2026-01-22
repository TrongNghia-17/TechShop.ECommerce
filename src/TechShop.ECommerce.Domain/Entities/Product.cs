namespace TechShop.ECommerce.Domain.Entities;

public class Product : BaseEntity
{
    public required string Name { get; set; }
    public string? Summary { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
