namespace TechShop.ECommerce.Application.Features.Products.GetProducts;

public sealed record ProductResponse(
    Guid Id,
    string Name,
    decimal Price,
    string? MainImageUrl);


