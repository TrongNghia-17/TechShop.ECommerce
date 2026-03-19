using TechShop.ECommerce.Domain.Entities.Catalogs;

namespace TechShop.ECommerce.Application.Features.Products.GetProductDetails;

public sealed class ProductDetailsMappingProfile : Profile
{
    public ProductDetailsMappingProfile()
    {
        CreateMap<Product, ProductDetailsDto>();
    }
}
