namespace TechShop.ECommerce.Application.Features.Products.Queries.GetDetails;

public sealed record GetProductDetailsQuery(Guid Id)
    : IRequest<Result<ProductDetailsDto>>;


