namespace TechShop.ECommerce.Application.Features.Orders.Shared;

public sealed class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
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