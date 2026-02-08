namespace TechShop.ECommerce.Application.Features.Products.Dtos;

public record ProductFeedItemDto(
    Guid Id,
    string Name,
    decimal Price,
    string CategoryName,
    DateTimeOffset DateCreated
);
