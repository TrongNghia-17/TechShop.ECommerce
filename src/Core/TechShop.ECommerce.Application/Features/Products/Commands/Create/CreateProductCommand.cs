namespace TechShop.ECommerce.Application.Features.Products.Commands.Create;

public sealed record CreateProductCommand(
    string Name,
    decimal Price,
    Guid CategoryId,
    string? Summary,
    string? Description
) : IRequest<Guid>;

