namespace TechShop.ECommerce.Application.Features.Orders.Commands.CreateOrder;

public sealed record PlaceOrderCommand(
    Guid CustomerId,
    AddressDto ShippingAddress,
    string? Notes
) : IRequest<Guid>;
