namespace TechShop.ECommerce.Application.Features.Product.Commands.UpdateProduct;

public sealed record UpdateProductCommand(
    int Id,
    string Name,
    decimal Price,
    int Quantity,
    int CategoryId,
    string? Summary,
    string? Description
) : IRequest<Unit>;

