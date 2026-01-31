namespace TechShop.ECommerce.Persistence.IntegrationTests;

public class TechShopDatabaseContextTests
{
    private TechShopDatabaseContext _techShopDatabaseContext;

    public TechShopDatabaseContextTests()
    {
        var dbOptions = new DbContextOptionsBuilder<TechShopDatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        _techShopDatabaseContext = new TechShopDatabaseContext(dbOptions);
    }

    [Fact]
    public async Task Save_Should_Set_CreatedDate()
    {
        // Arrange
        var product = Product.Create(
            name: "Test Product",
            price: 100_000m,
            categoryId: 1
        );

        // Act
        await _techShopDatabaseContext.Products.AddAsync(product);
        await _techShopDatabaseContext.SaveChangesAsync();

        // Assert
        product.CreatedDate.ShouldNotBe(default);
        product.IsDeleted.ShouldBeFalse();
    }

    [Fact]
    public async Task Update_Should_Set_UpdatedDate()
    {
        // Arrange
        var product = Product.Create(
            name: "Test Product",
            price: 100_000m,
            categoryId: 1
        );

        await _techShopDatabaseContext.Products.AddAsync(product);
        await _techShopDatabaseContext.SaveChangesAsync();

        var createdDate = product.CreatedDate;

        // Act
        product.Rename("Updated Product");
        await _techShopDatabaseContext.SaveChangesAsync();

        // Assert
        product.UpdatedDate.ShouldNotBeNull();
        product.UpdatedDate!.Value.ShouldBeGreaterThan(createdDate);
    }

    [Fact]
    public async Task Delete_Should_Set_IsDeleted_And_UpdatedDate()
    {
        // Arrange
        var product = Product.Create(
            name: "Test Product",
            price: 100_000m,
            categoryId: 1
        );

        await _techShopDatabaseContext.Products.AddAsync(product);
        await _techShopDatabaseContext.SaveChangesAsync();

        // Act
        _techShopDatabaseContext.Products.Remove(product);
        await _techShopDatabaseContext.SaveChangesAsync();

        // Assert
        product.IsDeleted.ShouldBeTrue();
        product.UpdatedDate.ShouldNotBeNull();
    }

}
