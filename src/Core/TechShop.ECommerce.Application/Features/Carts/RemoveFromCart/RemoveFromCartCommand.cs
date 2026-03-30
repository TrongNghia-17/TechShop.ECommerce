using TechShop.ECommerce.Application.Common.Results;
using TechShop.ECommerce.Application.Features.Carts.Shared;

namespace TechShop.ECommerce.Application.Features.Carts.RemoveFromCart;

public sealed record RemoveFromCartCommand(
    Guid ProductId,
    int Quantity
) : IRequest<Result<CartSummaryResponse>>;
