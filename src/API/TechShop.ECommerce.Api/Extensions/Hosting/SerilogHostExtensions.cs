namespace TechShop.ECommerce.Api.Extensions.Hosting;

public static class SerilogHostExtensions
{
    public static void AddSerilogLogging(
        this IHostApplicationBuilder builder)
    {
        builder.Services.AddSerilog((services, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(builder.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext();
        });
    }
}
