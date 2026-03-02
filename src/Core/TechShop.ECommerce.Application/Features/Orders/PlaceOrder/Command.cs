using TechShop.ECommerce.Application.Features.Orders.Shared;

namespace TechShop.ECommerce.Application.Features.Orders.PlaceOrder;

public sealed record Command(
    AddressDto ShippingAddress,
    string? Notes
) : IRequest<Result<Guid>>;
