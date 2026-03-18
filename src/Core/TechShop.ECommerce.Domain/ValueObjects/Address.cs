using TechShop.ECommerce.Domain.Exceptions;

namespace TechShop.ECommerce.Domain.ValueObjects;

public sealed class Address
{
    public string Street { get; private set; } = default!;
    public string City { get; private set; } = default!;
    public string PostalCode { get; private set; } = default!;
    public string Country { get; private set; } = default!;

    private Address() { }

    public Address(
        string street,
        string city,
        string postalCode,
        string country)
    {
        if (string.IsNullOrWhiteSpace(street))
        {
            throw new DomainException("Street is required.");
        }

        if (string.IsNullOrWhiteSpace(city))
        {
            throw new DomainException("City is required.");
        }

        if (string.IsNullOrWhiteSpace(country))
        {
            throw new DomainException("Country is required.");
        }

        Street = street.Trim();
        City = city.Trim();
        PostalCode = postalCode?.Trim() ?? string.Empty;
        Country = country.Trim();
    }
}
