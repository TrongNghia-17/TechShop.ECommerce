using TechShop.ECommerce.Application.Common.Results;
using TechShop.ECommerce.Application.Features.Carts.Shared;

namespace TechShop.ECommerce.Application.Features.Carts.AddToCart;

public sealed record AddToCartCommand(
    Guid ProductId,
    int Quantity
) : IRequest<Result<CartSummaryResponse>>;
