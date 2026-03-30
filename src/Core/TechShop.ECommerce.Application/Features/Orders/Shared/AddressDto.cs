namespace TechShop.ECommerce.Application.Features.Orders.Shared;

public sealed record AddressDto(
    string Street,
    string City,
    string PostalCode,
    string Country
);
