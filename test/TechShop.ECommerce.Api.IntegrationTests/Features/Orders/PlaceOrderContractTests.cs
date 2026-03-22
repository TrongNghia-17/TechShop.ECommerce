using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json;
using TechShop.ECommerce.Api.IntegrationTests.Setup;
using TechShop.ECommerce.Domain.Entities.Carts;
using TechShop.ECommerce.Domain.Entities.Catalogs;
using TechShop.ECommerce.Persistence.Context;

namespace TechShop.ECommerce.Api.IntegrationTests.Features.Orders;

public class PlaceOrderContractTests(CustomApiFactory factory) : IClassFixture<CustomApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task PlaceOrder_Response_ShouldFollowStrictApiContract()
    {
        // ARRANGE
        var customerId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        using (var scope = factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TechShopDbContext>();

            var fakeCategory = Category.Create("Electronics", "Danh mục test");
            dbContext.Categories.Add(fakeCategory);

            var categoryId = Guid.NewGuid();

            var fakeProduct = Product.Create(
                    name: "Laptop Gaming Test",
                    price: 50000m,
                    stockQuantity: 100,
                    categoryId: fakeCategory.Id
                );
            dbContext.Products.Add(fakeProduct);

            var oldCart = await dbContext.Carts.FirstOrDefaultAsync(c => c.CustomerId == customerId);
            if (oldCart != null)
            {
                dbContext.Carts.Remove(oldCart);
            }

            var fakeCart = Cart.Create(customerId);
            fakeCart.AddItem(fakeProduct.Id, fakeProduct.Price, 2);
            dbContext.Carts.Add(fakeCart);

            await dbContext.SaveChangesAsync();
        }

        var requestPayload = new
        {
            ShippingAddress = new
            {
                Street = "123 Đường Nguyễn Huệ",
                City = "TP.HCM",
                PostalCode = "700000",
                Country = "Vietnam"
            },
            Notes = "Đơn hàng test từ Integration Test"
        };

        var content = new StringContent(
            JsonSerializer.Serialize(requestPayload),
            Encoding.UTF8,
            "application/json");

        var response = await _client.PostAsync("/api/orders", content);

        var jsonString = await response.Content.ReadAsStringAsync();

        response.IsSuccessStatusCode.Should().BeTrue(
                $"API did not return a success status code (200/201). Status code: {response.StatusCode}. Server details: {jsonString}");

        var jsonDoc = JsonDocument.Parse(jsonString);
        var root = jsonDoc.RootElement;

        // ASSERT
        root.TryGetProperty("orderId", out var orderId).Should().BeTrue("API Contract strictly requires the 'orderId' field to be returned.");
        orderId.ValueKind.Should().Be(JsonValueKind.String, "Contract dictates that 'orderId' must be of type String (Guid).");

        root.TryGetProperty("checkoutUrl", out var checkoutUrlProperty).Should().BeTrue("API Contract strictly requires the 'checkoutUrl' field to be returned.");
        checkoutUrlProperty.ValueKind.Should().Be(JsonValueKind.String, "Contract dictates that 'checkoutUrl' must be of type String (URL).");

        checkoutUrlProperty.GetString().Should().NotBeNullOrWhiteSpace("Checkout URL must not be null or whitespace.");
    }
}
