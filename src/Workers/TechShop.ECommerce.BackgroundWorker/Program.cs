using Hangfire;
using Hangfire.PostgreSql;
using Serilog;
using TechShop.ECommerce.Application.BackgroundJobs;
using TechShop.ECommerce.Application.Contracts.Identity;
using TechShop.ECommerce.Identity.DependencyInjection;
using TechShop.ECommerce.Infrastructure.Background;
using TechShop.ECommerce.Infrastructure.DependencyInjection;
using TechShop.ECommerce.Infrastructure.Jobs.Orders;
using TechShop.ECommerce.Persistence.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog((services, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

builder.Services.AddBackgroundJobApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);

//builder.Services.AddCachingInfrastructure(builder.Configuration);
builder.Services.AddEmailInfrastructure();
builder.Services.AddBackgroundJobInfrastructure();

builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddScoped<ICurrentUserService, BackgroundCurrentUserService>();

builder.Services.AddHangfire(config =>
{
    config.UsePostgreSqlStorage(options =>
        options.UseNpgsqlConnection(
            builder.Configuration.GetConnectionString("DefaultConnection")));
});

builder.Services.AddHangfireServer(options =>
{
    options.WorkerCount = builder.Configuration.GetValue("Hangfire:WorkerCount", 5);
});

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

    recurringJobManager.AddOrUpdate<IHangfireOrderMaintenanceJobExecutor>(
        "expire-pending-orders",
        executor => executor.ExpirePendingOrders(),
        "*/5 * * * *");
}

await host.RunAsync();