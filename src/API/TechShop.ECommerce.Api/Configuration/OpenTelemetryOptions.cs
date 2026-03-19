using System.ComponentModel.DataAnnotations;

namespace TechShop.ECommerce.Api.Configuration;

public sealed class OpenTelemetryOptions
{
    public const string SectionName = "OpenTelemetry";

    [Required]
    public string ServiceName { get; init; } = "techshop-api";

    [Required]
    public string ServiceVersion { get; init; } = "1.0.0";

    [Required]
    public string OtlpEndpoint { get; init; } = "http://localhost:4317";
}
