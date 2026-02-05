var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.AddSerilog();

builder.Services
    .AddApiCore(builder.Configuration)
    .AddRateLimitingPolicies()
    .AddOutputCachingPolicies()
    .AddApiVersioningWithExplorer()
    .AddCorsAll()
    .AddOpenApiWithVersioning();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<TechShopDatabaseContext>();

    await db.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.

app.UseApiPipeline();

app.Run();
