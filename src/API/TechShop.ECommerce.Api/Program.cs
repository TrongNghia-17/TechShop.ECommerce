using TechShop.ECommerce.Api.Extensions.DependencyInjection;
using TechShop.ECommerce.Api.Extensions.Hosting;
using TechShop.ECommerce.Api.Extensions.Pipeline;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogLogging();

builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddApiHealthChecks();
builder.Services.AddApiRateLimiting();

var app = builder.Build();

await app.ApplyDevelopmentDatabaseInitializationAsync();
app.UseApiPipeline();

await app.RunAsync();

public partial class Program { }