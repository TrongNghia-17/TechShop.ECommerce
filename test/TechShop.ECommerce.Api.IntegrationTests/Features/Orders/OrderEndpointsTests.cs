using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using TechShop.ECommerce.Api.IntegrationTests.Setup;
using TechShop.ECommerce.Application.Features.Orders.PlaceOrder;
using TechShop.ECommerce.Application.Features.Orders.Shared;
using TechShop.ECommerce.Domain.Entities.Carts;
using TechShop.ECommerce.Domain.Entities.Catalogs;
using TechShop.ECommerce.Persistence.Context;

namespace TechShop.ECommerce.Api.IntegrationTests.Features.Orders;

[Collection("Shared Test Collection")]
public class OrderEndpointsTests
{
    private readonly HttpClient _client;
    private readonly CustomApiFactory _factory;

    public OrderEndpointsTests(CustomApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("TestScheme", "FakeToken");
    }

    [Fact]
    public async Task PlaceOrder_WithValidData_ShouldReturn201Created()
    {
        var userId = Guid.NewGuid();

        _client.DefaultRequestHeaders.Add("X-Test-User-Id", userId.ToString());

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<TechShopDbContext>();

            var category = Category.Create($"Electronics_{Guid.NewGuid()}", "Tech products");
            db.Categories.Add(category);
            await db.SaveChangesAsync();

            var product = Product.Create($"Laptop_{Guid.NewGuid()}", 1000m, 10, category.Id, "Desc");
            db.Products.Add(product);

            var cart = Cart.Create(userId);
            cart.AddItem(product.Id, 1000m, 1);
            db.Carts.Add(cart);

            await db.SaveChangesAsync();
        }

        // Arrange
        var command = new PlaceOrderCommand(
            new AddressDto("123 Le Loi", "Ho Chi Minh", "700000", "Vietnam"),
            "Giao trong giờ hành chính"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/orders", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        response.Headers.Location?.ToString().Should().StartWith("/api/orders/");
    }
}
