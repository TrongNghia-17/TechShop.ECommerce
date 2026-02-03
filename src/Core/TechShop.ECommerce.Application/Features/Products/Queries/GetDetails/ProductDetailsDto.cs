namespace TechShop.ECommerce.Application.Features.Products.Queries.GetDetails;

public sealed record ProductDetailsDto(
    int Id,
    string Name,
    string? Summary,
    string? Description,
    decimal Price,
    int CategoryId,
    DateTime DateCreated,
    DateTime? DateModified
);

