namespace TechShop.ECommerce.Application.Features.Products.Dtos;

public sealed record ProductCursor(
    DateTimeOffset DateCreated,
    Guid Id
);

