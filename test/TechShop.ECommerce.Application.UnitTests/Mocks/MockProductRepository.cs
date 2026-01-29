namespace TechShop.ECommerce.Application.UnitTests.Mocks;

public static class MockProductRepository
{
    public static Mock<IProductRepository> GetMockProductRepository()
    {
        var productDtos = new List<ProductDto>
        {
            new(1, "Product 1", 100, "Category A"),
            new(2, "Product 2", 200, "Category B")
        };

        var mockRepo = new Mock<IProductRepository>();

        mockRepo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(productDtos);

        return mockRepo;
    }
}
