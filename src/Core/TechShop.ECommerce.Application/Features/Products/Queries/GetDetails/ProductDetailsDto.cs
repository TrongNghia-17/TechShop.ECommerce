namespace TechShop.ECommerce.Application.Features.Products.Queries.GetDetails;

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

