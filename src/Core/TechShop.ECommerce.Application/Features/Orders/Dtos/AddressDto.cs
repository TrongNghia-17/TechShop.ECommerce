namespace TechShop.ECommerce.Application.Features.Orders.Dtos;

public sealed record AddressDto(
    string Street,
    string City,
    string PostalCode,
    string Country
);
