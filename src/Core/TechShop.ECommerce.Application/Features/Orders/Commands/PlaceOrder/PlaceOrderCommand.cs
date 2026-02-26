namespace TechShop.ECommerce.Application.Features.Orders.Commands.PlaceOrder;

public sealed record PlaceOrderCommand(
    AddressDto ShippingAddress,
    string? Notes
) : IRequest<Guid>;
