using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TechShop.ECommerce.Application.Common.Constants;
using TechShop.ECommerce.Identity.Context;
using TechShop.ECommerce.Migrations.Configuration;

namespace TechShop.ECommerce.Migrations.Identity;

public sealed class TechShopIdentityDbContextFactory : IDesignTimeDbContextFactory<TechShopIdentityDbContext>
{
    public TechShopIdentityDbContext CreateDbContext(string[] args)
    {
        var configuration = DesignTimeConfiguration.Build();
        var connectionString = DesignTimeConfiguration.GetRequiredConnectionString(
            configuration,
            ConnectionStrings.Default);

        var optionsBuilder = new DbContextOptionsBuilder<TechShopIdentityDbContext>();

        optionsBuilder.UseNpgsql(
            connectionString,
            postgresOptions =>
                postgresOptions.MigrationsAssembly(typeof(Program).Assembly.GetName().Name));

        return new TechShopIdentityDbContext(optionsBuilder.Options);
    }
}