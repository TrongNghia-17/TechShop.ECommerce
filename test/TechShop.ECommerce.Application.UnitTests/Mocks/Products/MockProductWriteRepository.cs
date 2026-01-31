using TechShop.ECommerce.Application.Contracts.Persistence;

namespace TechShop.ECommerce.Application.UnitTests.Mocks.Products;

public static class MockProductWriteRepository
{
    public static Mock<IProductRepository> GetMock()
    {
        var mockRepo = new Mock<IProductRepository>();

        mockRepo
            .Setup(r => r.ExistsByNameAsync(
                It.IsAny<string>(),
                It.IsAny<int?>()))
            .ReturnsAsync(false);

        return mockRepo;
    }
}
