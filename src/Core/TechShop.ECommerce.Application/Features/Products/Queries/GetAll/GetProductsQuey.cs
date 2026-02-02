using TechShop.ECommerce.Application.Features.Products.Dtos;

namespace TechShop.ECommerce.Application.Features.Products.Queries.GetAll;

public sealed record GetProductsQuery : IRequest<IReadOnlyList<ProductDto>>;
