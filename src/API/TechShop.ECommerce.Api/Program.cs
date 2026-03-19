var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.AddSerilog();

builder.Services
    .AddApiServices(builder.Configuration)
    .AddApiRateLimiting()
    .AddApiHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.

await app.ApplyDatabaseMigrationsAndSeedingAsync();

app.UseApiPipeline();

app.Run();
