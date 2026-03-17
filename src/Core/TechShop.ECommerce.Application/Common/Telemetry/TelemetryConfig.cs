namespace TechShop.ECommerce.Application.Common.Telemetry;

public static class TelemetryConfig
{
    public const string ActivitySourceName = "techshop-api";

    public static readonly ActivitySource ActivitySource =
        new(ActivitySourceName);
}