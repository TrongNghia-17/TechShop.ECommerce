namespace TechShop.ECommerce.Application.Features.Products.Queries.GetProducts;

public sealed record ProductDto(
    Guid Id,
    string Name,
    decimal Price,
    string CategoryName
);


