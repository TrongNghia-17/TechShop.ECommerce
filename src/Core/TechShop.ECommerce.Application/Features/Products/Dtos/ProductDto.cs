namespace TechShop.ECommerce.Application.Features.Products.Dtos;

public sealed record ProductDto(
    int Id,
    string Name,
    decimal Price,
    string CategoryName
);


