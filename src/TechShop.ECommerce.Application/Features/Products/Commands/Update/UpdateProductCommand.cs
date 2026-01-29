namespace TechShop.ECommerce.Application.Features.Products.Commands.Update;

public sealed record UpdateProductCommand(
    int Id,
    string Name,
    decimal Price,
    int CategoryId,
    string? Summary,
    string? Description
) : IRequest<Unit>;

