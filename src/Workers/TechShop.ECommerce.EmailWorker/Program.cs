using Hangfire;
using Hangfire.PostgreSql;
using TechShop.ECommerce.Application;
using TechShop.ECommerce.Infrastructure;
using TechShop.ECommerce.Persistence;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("../CommonConfig/appsettings.shared.json", optional: false)
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);

builder.Services.AddHangfire(config =>
{
    config.UsePostgreSqlStorage(options =>
        options.UseNpgsqlConnection(
            builder.Configuration.GetConnectionString("DefaultConnection")));
});

builder.Services.AddHangfireServer();

var host = builder.Build();
await host.RunAsync();