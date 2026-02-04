namespace TechShop.ECommerce.Application.Features.Products.Commands.Update;

public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    decimal Price,
    Guid CategoryId,
    string? Summary,
    string? Description
) : IRequest<Unit>;

