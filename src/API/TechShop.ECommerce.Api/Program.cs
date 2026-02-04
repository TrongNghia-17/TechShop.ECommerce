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

// Configure the HTTP request pipeline.

app.UseApiPipeline();

app.Run();
