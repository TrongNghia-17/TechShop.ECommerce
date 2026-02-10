namespace TechShop.ECommerce.Application.Features.Products.Commands.BulkUpdatePrice;

public sealed record BulkUpdatePriceCommand(
    Guid CategoryId,
    decimal PercentageChange
) : IRequest<Unit>;

