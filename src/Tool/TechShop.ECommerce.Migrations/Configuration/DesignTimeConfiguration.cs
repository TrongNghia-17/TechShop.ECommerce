using Microsoft.Extensions.Configuration;

namespace TechShop.ECommerce.Migrations.Configuration;

public static class DesignTimeConfiguration
{
    public static IConfiguration Build()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<Program>(optional: true)
            .AddEnvironmentVariables()
            .Build();
    }

    public static string GetRequiredConnectionString(IConfiguration configuration, string name)
    {
        return configuration.GetConnectionString(name)
            ?? throw new InvalidOperationException($"Connection string '{name}' was not found.");
    }
}