using TechShop.ECommerce.Application.Common.Results;
using TechShop.ECommerce.Application.Features.Orders.Shared;

namespace TechShop.ECommerce.Application.Features.Orders.PlaceOrder;

public sealed record PlaceOrderCommand(
    AddressDto ShippingAddress,
    string? Notes
) : IRequest<Result<PlaceOrderResponse>>;
