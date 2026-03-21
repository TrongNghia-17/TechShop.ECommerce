using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechShop.ECommerce.Application.Contracts.PaymentGateway;
using TechShop.ECommerce.Persistence.Context;
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
        builder.ConfigureServices(services =>
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "TestScheme";
                options.DefaultChallengeScheme = "TestScheme";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });

            var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<TechShopDbContext>));

            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            services.AddDbContext<TechShopDbContext>(options =>
            {
                options.UseNpgsql(_dbContainer.GetConnectionString());
            });

            var paymentDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IPaymentService));
            if (paymentDescriptor != null) services.Remove(paymentDescriptor);

            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock
                .Setup(x => x.CreateCheckoutSessionAsync(It.IsAny<Guid>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CheckoutSessionResult("mock_session_id", "https://checkout.stripe.com/test", "USD"));

            services.AddSingleton(paymentServiceMock.Object);
        });
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();

        await base.DisposeAsync();
    }
}
