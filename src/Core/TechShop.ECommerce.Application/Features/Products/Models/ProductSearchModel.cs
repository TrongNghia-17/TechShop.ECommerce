namespace TechShop.ECommerce.Application.Features.Products.Models;

public record CategorySearchModel(
    string Id, 
    string Name
);

public record ProductSearchModel(
    string Id,
    string Name,
    string? Description,
    string? ImageFile,
    decimal Price,
    CategorySearchModel Category
);
