namespace TechShop.ECommerce.Application.MappingProfiles;

public class ProductProfiles : Profile
{
    public ProductProfiles()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));
        CreateMap<Product, ProductDetailsDto>();
        CreateMap<CreateProductCommand, Product>();
    }
}
