namespace TechShop.ECommerce.Application.Features.Products.Dtos;

public sealed record ProductDto(
    Guid Id,
    string Name,
    decimal Price,
    string CategoryName
);


