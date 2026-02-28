namespace TechShop.ECommerce.Application.MappingProfiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDetailsDto>();
    }
}
