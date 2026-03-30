using TechShop.ECommerce.Application.Common.Results;

namespace TechShop.ECommerce.Application.Features.Carts.GetCart;

public sealed record GetCartQuery : IRequest<Result<GetCartResponse>>;
