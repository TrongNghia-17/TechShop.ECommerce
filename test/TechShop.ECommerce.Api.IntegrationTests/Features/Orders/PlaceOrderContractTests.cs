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

[Collection("Shared Test Collection")]
public class PlaceOrderContractTests(CustomApiFactory factory)
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task PlaceOrder_Response_ShouldFollowStrictApiContract()
    {
        // ARRANGE
        var customerId = Guid.NewGuid();

        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("TestScheme", "FakeToken");
        _client.DefaultRequestHeaders.Add("X-Test-User-Id", customerId.ToString());

        using (var scope = factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TechShopDbContext>();

            var fakeCategory = Category.Create($"Electronics_{Guid.NewGuid()}", "Danh mục test");
            dbContext.Categories.Add(fakeCategory);

            var fakeProduct = Product.Create($"Laptop_{Guid.NewGuid()}", 50000m, 100, fakeCategory.Id);
            dbContext.Products.Add(fakeProduct);

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
