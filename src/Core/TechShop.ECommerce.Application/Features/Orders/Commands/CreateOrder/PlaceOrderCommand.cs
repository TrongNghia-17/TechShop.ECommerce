namespace TechShop.ECommerce.Application.Features.Orders.Commands.CreateOrder;

public sealed record PlaceOrderCommand(
    Guid CustomerId,
    List<OrderItemDto> Items,
    AddressDto ShippingAddress,
    string? Notes
) : IRequest<Guid>;
