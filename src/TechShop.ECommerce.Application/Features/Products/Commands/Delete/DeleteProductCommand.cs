namespace TechShop.ECommerce.Application.Features.Products.Commands.Delete;

public sealed record DeleteProductCommand(int Id) : IRequest<Unit>;

