namespace TechShop.ECommerce.Application.Features.Products.Queries.GetProducts;

public sealed record GetProductsProjection(
    Guid Id,
    string Name,
    decimal Price,
    string CategoryName,
    string? MainImageBlobName);
