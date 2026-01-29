namespace TechShop.ECommerce.Application.Features.Products.Commands.Create;

public sealed record CreateProductCommand(
    string Name,
    decimal Price,
    int CategoryId,
    string? Summary,
    string? Description
) : IRequest<int>;

