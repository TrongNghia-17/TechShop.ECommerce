namespace TechShop.ECommerce.Application.MappingProfiles;

public class ProductProfiles : Profile
{
    public ProductProfiles()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<Product, ProductDetailsDto>();
        CreateMap<CreateProductCommand, Product>();
        CreateMap<UpdateProductCommand, Product>()
            .ForMember(p => p.CreatedDate, opt => opt.Ignore())
            .ForMember(p => p.UpdatedDate, opt => opt.Ignore())
            .ForMember(p => p.IsDeleted, opt => opt.Ignore());
    }
}
