namespace TechShop.ECommerce.Application.Features.Products.Queries.GetAll;

public sealed record ProductDto(
    int Id,
    string Name,
    decimal Price,
    string CategoryName
);


