var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.AddSerilog();

builder.Services
    .AddApiCore(builder.Configuration)
    .AddRateLimitingPolicies()
    .AddOutputCachingPolicies()
    .AddCorsAll();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var services = scope.ServiceProvider;

    var db = services.GetRequiredService<TechShopDatabaseContext>();
    await db.Database.MigrateAsync();

    var identityDb = services.GetRequiredService<TechShopIdentityDbContext>();
    await identityDb.Database.MigrateAsync();

    await IdentitySeederRunner.SeedAsync(services);
    await TechShopDataSeeder.SeedAsync(services);
}

// Configure the HTTP request pipeline.

app.UseApiPipeline();

app.Run();
