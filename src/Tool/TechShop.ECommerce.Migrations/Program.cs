using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TechShop.ECommerce.Identity;
using TechShop.ECommerce.Identity.DbContext;
using TechShop.ECommerce.Identity.Seeding;
using TechShop.ECommerce.Persistence;
using TechShop.ECommerce.Persistence.DatabaseContext;
using TechShop.ECommerce.Persistence.Seeding;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddIdentityCoreServices(builder.Configuration);

using var app = builder.Build();
using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;
var cancellationToken = CancellationToken.None;

var appDbContext = services.GetRequiredService<TechShopDbContext>();
await appDbContext.Database.MigrateAsync(cancellationToken);

var identityDbContext = services.GetRequiredService<TechShopIdentityDbContext>();
await identityDbContext.Database.MigrateAsync(cancellationToken);

await IdentitySeederRunner.SeedAsync(services, cancellationToken);
await TechShopSeederRunner.SeedAsync(services, cancellationToken);

Console.WriteLine("Database migration completed successfully.");