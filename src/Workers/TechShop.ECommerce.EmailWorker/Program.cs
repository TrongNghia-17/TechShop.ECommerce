using Hangfire;
using Hangfire.PostgreSql;
using TechShop.ECommerce.Application;
using TechShop.ECommerce.Identity;
using TechShop.ECommerce.Infrastructure;
using TechShop.ECommerce.Persistence;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddHangfire(config =>
{
    config.UsePostgreSqlStorage(options =>
        options.UseNpgsqlConnection(
            builder.Configuration.GetConnectionString("DefaultConnection")));
});

builder.Services.AddHangfireServer();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("../CommonConfig/appsettings.shared.json", optional: false)
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables();

var host = builder.Build();
await host.RunAsync();