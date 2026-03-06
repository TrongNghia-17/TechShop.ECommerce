using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TechShop.ECommerce.Persistence.DatabaseContext;

namespace TechShop.ECommerce.Migrations.AppDb;

public class TechShopDbContextFactory
    : IDesignTimeDbContextFactory<TechShopDbContext>
{
    public TechShopDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        var optionsBuilder = new DbContextOptionsBuilder<TechShopDbContext>();
        optionsBuilder.UseNpgsql(
            connectionString,
             postgresOptions =>
             {
                 postgresOptions.MigrationsAssembly(
                     typeof(TechShopDbContextFactory).Assembly.GetName().Name);
             });

        return new TechShopDbContext(optionsBuilder.Options);
    }

    private static IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<TechShopDbContextFactory>()
            .AddEnvironmentVariables()
            .Build();
    }
}