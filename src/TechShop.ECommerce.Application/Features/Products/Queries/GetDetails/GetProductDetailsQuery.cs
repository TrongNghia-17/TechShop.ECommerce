namespace TechShop.ECommerce.Application.Features.Products.Queries.GetDetails;

public sealed record GetProductDetailsQuery(int Id)
    : IRequest<ProductDetailsDto>;


