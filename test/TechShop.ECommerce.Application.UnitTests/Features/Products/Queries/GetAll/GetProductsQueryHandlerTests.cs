using TechShop.ECommerce.Application.Contracts.Logging;
using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Application.Features.Products.Queries.GetAll;

namespace TechShop.ECommerce.Application.UnitTests.Features.Products.Queries.GetAll;

public class GetProductsQueryHandlerTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IAppLogger<GetProductsQueryHandler>> _mockLogger;

    public GetProductsQueryHandlerTests()
    {
        _mockProductRepository = MockProductReadRepository.GetMock();
        _mockLogger = new Mock<IAppLogger<GetProductsQueryHandler>>();
    }

    [Fact]
    public async Task Handle_ShouldReturnAllProducts_AndLogInformation()
    {
        // Arrange
        var handler = new GetProductsQueryHandler(
            _mockProductRepository.Object,
            _mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetProductsQuery(), CancellationToken.None);

        // Assert - type + count
        result.ShouldBeAssignableTo<IReadOnlyList<ProductDto>>();
        result.Count.ShouldBe(2);

        // Assert - data 
        result[0].Id.ShouldBe(1);
        result[0].Name.ShouldBe("Product 1");
        result[0].Price.ShouldBe(100);
        result[0].CategoryName.ShouldBe("Category A");

        result[1].Id.ShouldBe(2);
        result[1].Name.ShouldBe("Product 2");
        result[1].Price.ShouldBe(200);
        result[1].CategoryName.ShouldBe("Category B");

        // Assert - behavior
        _mockProductRepository.Verify(r => r.GetAllAsync(), Times.Once);

        _mockLogger.Verify(
            l => l.LogInformation("Retrieving products"),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoProductsExist()
    {
        // Arrange
        _mockProductRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<ProductDto>());

        var handler = new GetProductsQueryHandler(
            _mockProductRepository.Object,
            _mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetProductsQuery(), CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(0);

        _mockProductRepository.Verify(r => r.GetAllAsync(), Times.Once);
        _mockLogger.Verify(l => l.LogInformation("Retrieving products"), Times.Once);
    }
}
