namespace TechShop.ECommerce.Application.MappingProfiles;

public sealed class OrderProfile : Profile
{
    public OrderProfile()
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