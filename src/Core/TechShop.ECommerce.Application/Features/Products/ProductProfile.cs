using TechShop.ECommerce.Application.Features.Products.Queries.GetProductDetails;

namespace TechShop.ECommerce.Application.Features.Products;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDetailsDto>();
    }
}
