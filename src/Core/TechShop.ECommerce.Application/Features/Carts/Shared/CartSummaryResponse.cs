namespace TechShop.ECommerce.Application.Features.Carts.Shared;

public sealed record CartSummaryResponse(
    Guid CartId,
    decimal Total
);
