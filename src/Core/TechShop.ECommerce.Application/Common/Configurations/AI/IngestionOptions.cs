using System.ComponentModel.DataAnnotations;

namespace TechShop.ECommerce.Application.Common.Configurations.AI;

public sealed class IngestionOptions
{
    public const string SectionName = "AI:Ingestion";
    [Range(1, 100)]
    public int BatchSize { get; set; } = 10;
    [Range(0, 5000)]
    public int BatchDelayMs { get; set; } = 500;
}
