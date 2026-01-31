namespace TechShop.ECommerce.Application.Features.Products.Queries.GetAll;

public sealed record GetProductsQuery : IRequest<IReadOnlyList<ProductDto>>;
