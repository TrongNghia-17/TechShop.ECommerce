using TechShop.ECommerce.Application.Features.Products.Shared;

namespace TechShop.ECommerce.Application.Features.AI.Shared;

public record SourceResult(
    string Type,
    string Content,
    string? Url = null
);

public record ChatResponse(
    string Answer,
    IReadOnlyList<ProductSearchModel>? Products = null,
    IReadOnlyList<SourceResult>? Sources = null
);
