namespace TechShop.ECommerce.Application.Features.Products.Queries.GetProducts;

public sealed record GetProductsResponse(
    Guid Id,
    string Name,
    decimal Price,
    string CategoryName,
    string? MainImageUrl);


