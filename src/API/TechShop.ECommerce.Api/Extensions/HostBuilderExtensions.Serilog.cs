namespace TechShop.ECommerce.Api.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder AddSerilog(this IHostBuilder host) =>
        host.UseSerilog((context, config) =>
            config.ReadFrom.Configuration(context.Configuration));
}
