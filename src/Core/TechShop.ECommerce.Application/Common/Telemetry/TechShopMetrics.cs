using System.Diagnostics.Metrics;

namespace TechShop.ECommerce.Application.Common.Telemetry;

public static class TechShopMetrics
{
    public const string MeterName = "TechShop.Business";

    public static readonly Meter Meter = new(MeterName);

    public static readonly Counter<int> OrdersCreated =
        Meter.CreateCounter<int>(
            name: "techshop_orders_created",
            description: "Total number of successfully created orders.");

    public static readonly Counter<int> LoginFailures =
        Meter.CreateCounter<int>(
            name: "techshop_login_failures",
            description: "Total number of failed login attempts.");

    public static readonly Counter<int> ProductImageUploads =
        Meter.CreateCounter<int>(
            name: "techshop_product_image_uploads",
            description: "Total number of successfully uploaded product images.");
}
