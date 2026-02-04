namespace TechShop.ECommerce.Application.Features.Products.Commands.Delete;

public sealed record DeleteProductCommand(Guid Id) : IRequest<Unit>;

