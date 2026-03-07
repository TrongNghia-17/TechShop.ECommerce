var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.AddSerilog();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("../CommonConfig/appsettings.shared.json", optional: false)
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services
    .AddApiServices(builder.Configuration)
    .AddRateLimitingPolicies();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseApiPipeline();

app.Run();
