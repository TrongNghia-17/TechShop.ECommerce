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

var app = builder.Build();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

var db = services.GetRequiredService<TechShopDbContext>();
await db.Database.MigrateAsync();

var identityDb = services.GetRequiredService<TechShopIdentityDbContext>();
await identityDb.Database.MigrateAsync();

await IdentitySeederRunner.SeedAsync(services);
await TechShopDataSeeder.SeedAsync(services);

Console.WriteLine("Database migration completed.");