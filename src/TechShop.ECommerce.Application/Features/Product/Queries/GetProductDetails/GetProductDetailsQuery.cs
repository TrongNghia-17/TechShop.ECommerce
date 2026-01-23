namespace TechShop.ECommerce.Application.Features.Product.Queries.GetProductDetails;

public record GetProductDetailsQuery(int Id) : IRequest<ProductDetailsDto>;

