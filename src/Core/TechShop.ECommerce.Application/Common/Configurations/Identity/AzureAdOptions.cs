namespace TechShop.ECommerce.Application.Common.Configurations.Identity;

public sealed class AzureAdOptions
{
    public const string SectionName = "AzureAd";

    public string Instance { get; set; } = default!;
    public string Domain { get; set; } = default!;
    public string TenantId { get; set; } = default!;
    public string ClientId { get; set; } = default!;
    public Dictionary<string, string> Scopes { get; set; } = new();
}
