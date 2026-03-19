using TechShop.ECommerce.Application.Common.Results;

namespace TechShop.ECommerce.Application.Features.Products.UpdateProduct;

public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    string Summary,
    decimal Price)
    : IRequest<Result>;