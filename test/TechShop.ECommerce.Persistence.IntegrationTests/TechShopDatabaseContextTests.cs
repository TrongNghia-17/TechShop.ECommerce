using Moq;
using TechShop.ECommerce.Application.Contracts.Identity;
using TechShop.ECommerce.Domain.Entities.Catalog;
using TechShop.ECommerce.Persistence.DatabaseContext;

namespace TechShop.ECommerce.Persistence.IntegrationTests;

public class TechShopDatabaseContextTests
{
    private readonly TechShopDatabaseContext _techShopDatabaseContext;
    private readonly string _userId;
    private readonly Mock<IUserService> _userServiceMock;

    public TechShopDatabaseContextTests()
    {
        var dbOptions = new DbContextOptionsBuilder<TechShopDatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        _userId = "00000000-0000-0000-0000-000000000000";
        _userServiceMock = new Mock<IUserService>();
        _userServiceMock.Setup(m => m.UserId).Returns(_userId);

        _techShopDatabaseContext = new TechShopDatabaseContext(dbOptions);
    }

    [Fact]
    public async Task Save_Should_Set_CreatedDate()
    {
        // Arrange
        var product = Product.Create(
            name: "Test Product",
            price: 100_000m,
            categoryId: Guid.NewGuid()
        );

        // Act
        await _techShopDatabaseContext.Products.AddAsync(product);
        await _techShopDatabaseContext.SaveChangesAsync();

        // Assert
        product.DateCreated.ShouldNotBe(default);
        product.IsDeleted.ShouldBeFalse();
    }

    [Fact]
    public async Task Update_Should_Set_UpdatedDate()
    {
        // Arrange
        var product = Product.Create(
            name: "Test Product",
            price: 100_000m,
            categoryId: Guid.NewGuid()
        );

        await _techShopDatabaseContext.Products.AddAsync(product);
        await _techShopDatabaseContext.SaveChangesAsync();

        var createdDate = product.DateCreated;

        // Act
        product.Rename("Updated Product");
        await _techShopDatabaseContext.SaveChangesAsync();

        // Assert
        product.DateModified.ShouldNotBeNull();
        product.DateModified!.Value.ShouldBeGreaterThan(createdDate);
    }

    [Fact]
    public async Task Delete_Should_Set_IsDeleted_And_UpdatedDate()
    {
        // Arrange
        var product = Product.Create(
            name: "Test Product",
            price: 100_000m,
            categoryId: Guid.NewGuid()
        );

        await _techShopDatabaseContext.Products.AddAsync(product);
        await _techShopDatabaseContext.SaveChangesAsync();

        // Act
        _techShopDatabaseContext.Products.Remove(product);
        await _techShopDatabaseContext.SaveChangesAsync();

        // Assert
        product.IsDeleted.ShouldBeTrue();
        product.DateModified.ShouldNotBeNull();
    }

}
