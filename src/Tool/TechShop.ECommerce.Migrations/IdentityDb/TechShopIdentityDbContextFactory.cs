using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TechShop.ECommerce.Identity.DbContext;
using TechShop.ECommerce.Migrations.AppDb;

namespace TechShop.ECommerce.Migrations.IdentityDb;

public class TechShopIdentityDbContextFactory
    : IDesignTimeDbContextFactory<TechShopIdentityDbContext>
{
    public TechShopIdentityDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        var optionsBuilder = new DbContextOptionsBuilder<TechShopIdentityDbContext>();
        optionsBuilder.UseNpgsql(
            connectionString,
            postgresOptions =>
            {
                postgresOptions.MigrationsAssembly(
                    typeof(TechShopDbContextFactory).Assembly.GetName().Name);
            });

        return new TechShopIdentityDbContext(optionsBuilder.Options);
    }

    private static IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<TechShopIdentityDbContextFactory>()
            .AddEnvironmentVariables()
            .Build();
    }
}