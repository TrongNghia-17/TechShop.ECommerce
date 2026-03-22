using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using TechShop.ECommerce.Application.Contracts.PaymentGateway;
using Testcontainers.PostgreSql;

namespace TechShop.ECommerce.Api.IntegrationTests.Setup;

public class CustomApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:15-alpine")
        .WithDatabase("TechShop_TestDb")
        .WithUsername("postgres")
        .WithPassword("StrongPassword123!")
        .Build();

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
        {
                { "JwtSettings:Key", "ThisIsADummyKeyForTestingPurposeOnly12345!" },

                { "EmailSettings:ApiKey", "dummy_email_api_key" },
                { "EmailSettings:FromAddress", "test@techshop.com" },

                { "StripeSettings:SecretKey", "sk_test_dummy" },
                { "StripeSettings:WebhookSecret", "whsec_dummy" },
                { "StripeSettings:SuccessUrl", "http://localhost/success" },
                { "StripeSettings:CancelUrl", "http://localhost/cancel" },
                { "StripeSettings:Currency", "usd" },

                { "AzureStorage:ConnectionString", "UseDevelopmentStorage=true;" },

                { "ConnectionStrings:DefaultConnection", _dbContainer.GetConnectionString() }
            });
        });

        builder.ConfigureServices(services =>
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "TestScheme";
                options.DefaultChallengeScheme = "TestScheme";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });

            services.RemoveAll(typeof(IPaymentService));
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock
                .Setup(x => x.CreateCheckoutSessionAsync(It.IsAny<Guid>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new CheckoutSessionResult($"mock_session_{Guid.NewGuid()}", "https://checkout.stripe.com/test", "USD"));

            services.AddSingleton(paymentServiceMock.Object);
        });
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
        await base.DisposeAsync();
    }
}
