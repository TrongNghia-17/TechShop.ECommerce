using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Application.Exceptions;
using TechShop.ECommerce.Application.Features.Carts.Commands.RemoveFromCart;
using TechShop.ECommerce.Application.UnitTests.Mocks.UnitOfWork;
using TechShop.ECommerce.Domain.Entities.Cart;

namespace TechShop.ECommerce.Application.UnitTests.Features.Carts.Commands.RemoveFromCart;

public class RemoveFromCartCommandHandlerTests
{
    private readonly Mock<ICartRepository> _cartRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public RemoveFromCartCommandHandlerTests()
    {
        _cartRepository = new Mock<ICartRepository>();
        _unitOfWork = MockUnitOfWork.GetMock();
    }

    [Fact]
    public async Task Handle_Should_Remove_Quantity_And_Return_Updated_Total()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var cart = Cart.Create(customerId);
        cart.AddItem(productId, 100m, 3);

        _cartRepository
            .Setup(r => r.GetByCustomerIdAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        var handler = new RemoveFromCartCommandHandler(_cartRepository.Object, _unitOfWork.Object);
        var command = new RemoveFromCartCommand(customerId, productId, 1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.CartId.ShouldBe(cart.Id);
        result.Total.ShouldBe(200m);
        cart.Items.Single().Quantity.ShouldBe(2);

        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFound_When_Cart_Does_Not_Exist()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var handler = new RemoveFromCartCommandHandler(_cartRepository.Object, _unitOfWork.Object);
        var command = new RemoveFromCartCommand(customerId, Guid.NewGuid(), 1);

        _cartRepository
            .Setup(r => r.GetByCustomerIdAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cart?)null);

        // Act
        var action = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await action.ShouldThrowAsync<NotFoundException>();
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
