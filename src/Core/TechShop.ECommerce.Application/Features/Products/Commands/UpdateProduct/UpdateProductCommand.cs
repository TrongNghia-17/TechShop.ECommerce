namespace TechShop.ECommerce.Application.Features.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    string Summary,
    decimal Price)
    : IRequest<Result>;