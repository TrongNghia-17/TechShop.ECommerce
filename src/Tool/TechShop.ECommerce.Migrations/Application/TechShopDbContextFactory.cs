using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TechShop.ECommerce.Application.Common.Constants;
using TechShop.ECommerce.Migrations.Configuration;
using TechShop.ECommerce.Persistence.Context;

namespace TechShop.ECommerce.Migrations.Application;

public sealed class TechShopDbContextFactory : IDesignTimeDbContextFactory<TechShopDbContext>
{
    public TechShopDbContext CreateDbContext(string[] args)
    {
        var configuration = DesignTimeConfiguration.Build();
        var connectionString = DesignTimeConfiguration.GetRequiredConnectionString(
            configuration,
            ConnectionStrings.Default);

        var optionsBuilder = new DbContextOptionsBuilder<TechShopDbContext>();

        optionsBuilder.UseNpgsql(
            connectionString,
            postgresOptions =>
                postgresOptions.MigrationsAssembly(typeof(Program).Assembly.GetName().Name));

        return new TechShopDbContext(optionsBuilder.Options);
    }
}