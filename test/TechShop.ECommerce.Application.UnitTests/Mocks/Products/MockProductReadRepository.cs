using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Application.Features.Products.Queries.GetAll;

namespace TechShop.ECommerce.Application.UnitTests.Mocks.Products;

public static class MockProductReadRepository
{
    public static Mock<IProductRepository> GetMock()
    {
        var productDtos = new List<ProductDto>
        {
            new(1, "Product 1", 100, "Category A"),
            new(2, "Product 2", 200, "Category B")
        };

        var mockRepo = new Mock<IProductRepository>();

        mockRepo
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(productDtos);

        return mockRepo;
    }
}
