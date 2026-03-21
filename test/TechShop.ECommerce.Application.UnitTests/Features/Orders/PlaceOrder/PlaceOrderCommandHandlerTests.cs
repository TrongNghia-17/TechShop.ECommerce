using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TechShop.ECommerce.Application.Common.Errors;
using TechShop.ECommerce.Application.Contracts.Identity;
using TechShop.ECommerce.Application.Contracts.PaymentGateway;
using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Application.Features.Orders.PlaceOrder;
using TechShop.ECommerce.Application.Features.Orders.Shared;
using TechShop.ECommerce.Domain.Entities.Carts;
using TechShop.ECommerce.Domain.Entities.Catalogs;
using TechShop.ECommerce.Domain.Entities.Orders;
using TechShop.ECommerce.Domain.Entities.Payments;
using TechShop.ECommerce.Domain.Errors;
using TechShop.ECommerce.Domain.ValueObjects;

namespace TechShop.ECommerce.Application.Tests.Features.Orders.PlaceOrder;

public sealed class PlaceOrderCommandHandlerTests
{
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
    private readonly Mock<IPaymentService> _paymentServiceMock = new();
    private readonly Mock<ICartRepository> _cartRepositoryMock = new();
    private readonly Mock<IProductRepository> _productRepositoryMock = new();
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly Mock<IPaymentRepository> _paymentRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<PlaceOrderCommandHandler>> _loggerMock = new();

    private readonly PlaceOrderCommandHandler _handler;

    public PlaceOrderCommandHandlerTests()
    {
        _handler = new PlaceOrderCommandHandler(
            _currentUserServiceMock.Object,
            _paymentServiceMock.Object,
            _cartRepositoryMock.Object,
            _productRepositoryMock.Object,
            _orderRepositoryMock.Object,
            _paymentRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var command = CreateCommand();
        _currentUserServiceMock.SetupGet(x => x.UserId).Returns(Guid.Empty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(IdentityErrors.Unauthorized);

        _cartRepositoryMock.Verify(x => x.GetByCustomerIdAsync(It.IsAny<Guid>(), default), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCartIsEmpty()
    {
        // Arrange
        var command = CreateCommand();
        SetupValidUser();

        var emptyCart = Cart.Create(Guid.NewGuid());
        _cartRepositoryMock.Setup(x => x.GetByCustomerIdAsync(It.IsAny<Guid>(), default))
            .ReturnsAsync(emptyCart);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OrderErrors.EmptyCart);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenProductIsOutOfStock()
    {
        // Arrange
        var command = CreateCommand();
        var customerId = SetupValidUser();

        var productId = Guid.NewGuid();
        var cart = Cart.Create(customerId);
        cart.AddItem(productId, 1000m, 5);

        _cartRepositoryMock.Setup(x => x.GetByCustomerIdAsync(customerId, default))
            .ReturnsAsync(cart);

        var product = Product.Create("Laptop", 1000m, 2, Guid.NewGuid(), "Desc");
        typeof(Product).GetProperty("Id")?.SetValue(product, productId);

        _productRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<IEnumerable<Guid>>(), default))
            .ReturnsAsync([product]);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProductErrors.InsufficientStock(productId));
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAllConditionsAreMet()
    {
        // Arrange
        var command = CreateCommand();
        var customerId = SetupValidUser();

        var productId = Guid.NewGuid();
        var cart = Cart.Create(customerId);
        cart.AddItem(productId, 1000m, 1);
        _cartRepositoryMock.Setup(x => x.GetByCustomerIdAsync(customerId, default)).ReturnsAsync(cart);

        var product = Product.Create("Laptop", 1000m, 10, Guid.NewGuid(), "Desc");
        typeof(Product).GetProperty("Id")?.SetValue(product, productId);
        _productRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<IEnumerable<Guid>>(), default)).ReturnsAsync([product]);

        _mapperMock.Setup(x => x.Map<Address>(command.ShippingAddress)).Returns(new Address("123 Le Loi", "HCM", "VN", "70000"));

        var checkoutResult = new CheckoutSessionResult("session_123", "https://stripe.com/pay", "USD");
        _paymentServiceMock.Setup(x => x.CreateCheckoutSessionAsync(
            It.IsAny<Guid>(),
            It.IsAny<decimal>(),
            It.IsAny<CancellationToken>()))
        .ReturnsAsync(checkoutResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.CheckoutUrl.Should().Be("https://stripe.com/pay");

        _orderRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Order>(), default), Times.Once);
        _paymentRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Payment>(), default), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Once);

        cart.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserEmailIsMissing()
    {
        // Arrange
        var command = CreateCommand();
        _currentUserServiceMock.SetupGet(x => x.UserId).Returns(Guid.NewGuid());
        _currentUserServiceMock.SetupGet(x => x.Email).Returns(string.Empty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(IdentityErrors.EmailNotFound);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCartIsNotFound()
    {
        // Arrange
        var command = CreateCommand();
        var customerId = SetupValidUser();

        _cartRepositoryMock.Setup(x => x.GetByCustomerIdAsync(customerId, default))
            .ReturnsAsync((Cart?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CartErrors.NotFound(customerId));
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenProductIsNotFound()
    {
        // Arrange
        var command = CreateCommand();
        var customerId = SetupValidUser();

        var productId = Guid.NewGuid();
        var cart = Cart.Create(customerId);
        cart.AddItem(productId, 1000m, 1);
        _cartRepositoryMock.Setup(x => x.GetByCustomerIdAsync(customerId, default))
            .ReturnsAsync(cart);

        _productRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<IEnumerable<Guid>>(), default))
            .ReturnsAsync([]);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProductErrors.NotFound(productId));
    }

    [Fact]
    public async Task Handle_ShouldSetActivityTag_WhenActivityIsActive()
    {
        // Arrange
        var command = CreateCommand();
        var customerId = SetupValidUser();

        var emptyCart = Cart.Create(customerId);
        _cartRepositoryMock.Setup(x => x.GetByCustomerIdAsync(customerId, default))
            .ReturnsAsync(emptyCart);

        using var activity = new System.Diagnostics.Activity("TestActivity").Start();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        activity.GetTagItem("customer.id").Should().Be(customerId);
    }

    private static PlaceOrderCommand CreateCommand()
    {
        return new PlaceOrderCommand(
            new AddressDto("123 Le Loi", "Ho Chi Minh", "700000", "Vietnam"),
            "Giao trong giờ hành chính"
        );
    }

    private Guid SetupValidUser()
    {
        var userId = Guid.NewGuid();
        _currentUserServiceMock.SetupGet(x => x.UserId).Returns(userId);
        _currentUserServiceMock.SetupGet(x => x.Email).Returns("student@hcmus.edu.vn");
        return userId;
    }
}
