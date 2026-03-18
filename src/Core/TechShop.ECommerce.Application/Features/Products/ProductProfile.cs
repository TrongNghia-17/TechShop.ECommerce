using TechShop.ECommerce.Application.Features.Products.Queries.GetProductDetails;
using TechShop.ECommerce.Domain.Entities.Catalogs;

namespace TechShop.ECommerce.Application.Features.Products;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDetailsDto>();
    }
}
