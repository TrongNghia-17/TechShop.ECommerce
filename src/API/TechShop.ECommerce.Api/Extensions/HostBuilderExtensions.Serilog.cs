namespace TechShop.ECommerce.Api.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder AddSerilog(this IHostBuilder host) =>
        host.UseSerilog((context, loggerConfig) => loggerConfig
            .WriteTo.Console()
            .ReadFrom.Configuration(context.Configuration));
}
