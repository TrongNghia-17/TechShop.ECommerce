namespace TechShop.ECommerce.Application.Features.Carts.Queries.GetCart;

public sealed record GetCartQuery(Guid CustomerId) : IRequest<GetCartResult>;
