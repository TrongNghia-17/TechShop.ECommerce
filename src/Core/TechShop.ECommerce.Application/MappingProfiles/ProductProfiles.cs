namespace TechShop.ECommerce.Application.MappingProfiles;

public class ProductProfiles : Profile
{
    public ProductProfiles()
    {
        CreateMap<Product, ProductDetailsDto>();
    }
}
