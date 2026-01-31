using TechShop.ECommerce.Application.Contracts.Logging;
using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Application.Features.Products.Commands.Create;
using TechShop.ECommerce.Application.UnitTests.Mocks.UnitOfWork;
using TechShop.ECommerce.Domain.Entities.Products;

namespace TechShop.ECommerce.Application.UnitTests.Features.Products.Commands.Create;

public class CreateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IAppLogger<CreateProductCommandHandler>> _logger;

    public CreateProductCommandHandlerTests()
    {
        _productRepository = MockProductWriteRepository.GetMock();
        _unitOfWork = MockUnitOfWork.GetMock();
        _logger = new Mock<IAppLogger<CreateProductCommandHandler>>();
    }

    [Fact]
    public async Task Handle_Should_Return_ProductId_When_Product_Is_Created()
    {
        // Arrange
        var handler = new CreateProductCommandHandler(
            _productRepository.Object,
            _unitOfWork.Object,
            _logger.Object);

        var command = new CreateProductCommand(
            Name: "Test Product",
            Price: 100m,
            CategoryId: 1,
            Summary: "Summary",
            Description: "Description"
        );

        Product? createdProduct = null;

        _productRepository
            .Setup(r => r.AddAsync(It.IsAny<Product>()))
            .Callback<Product>(product =>
            {
                product.SetId(1);
                createdProduct = product;
            })
            .Returns(Task.CompletedTask);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert - Result
        result.ShouldBe(1);

        // Assert - Interactions
        _productRepository.Verify(
            r => r.AddAsync(It.IsAny<Product>()),
            Times.Once);

        _unitOfWork.Verify(
            u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        // Assert - Domain mapping
        createdProduct.ShouldNotBeNull();
        createdProduct!.Name.ShouldBe(command.Name);
        createdProduct.Price.ShouldBe(command.Price);
        createdProduct.CategoryId.ShouldBe(command.CategoryId);
    }
}

