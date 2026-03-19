namespace TechShop.ECommerce.Application.Features.Products.GetProductDetails;

public sealed record ProductDetailsDto(
    Guid Id,
    string Name,
    string? Summary,
    string? Description,
    decimal Price,
    Guid CategoryId,
    DateTimeOffset DateCreated,
    DateTimeOffset? DateModified
);

