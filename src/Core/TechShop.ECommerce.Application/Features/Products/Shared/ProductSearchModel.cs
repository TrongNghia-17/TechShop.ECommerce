namespace TechShop.ECommerce.Application.Features.Products.Shared;

public record CategorySearchModel(
    string Id, 
    string Name
);

public record ProductSearchModel(
    string Id,
    string Name,
    string? Description,
    string? ImageFile,
    decimal Price,
    CategorySearchModel Category,
    float Score = 0 // <-- Thêm điểm số mặc định
);
