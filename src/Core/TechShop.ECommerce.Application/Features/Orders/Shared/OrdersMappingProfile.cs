using TechShop.ECommerce.Domain.ValueObjects;

namespace TechShop.ECommerce.Application.Features.Orders.Shared;

public sealed class OrdersMappingProfile : Profile
{
    public OrdersMappingProfile()
    {
        CreateMap<AddressDto, Address>()
            .ConstructUsing(src => new Address(
                src.Street,
                src.City,
                src.PostalCode,
                src.Country
            ));
    }
}