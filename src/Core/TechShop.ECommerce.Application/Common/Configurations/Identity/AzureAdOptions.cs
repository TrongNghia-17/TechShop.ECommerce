namespace TechShop.ECommerce.Application.Common.Configurations.Identity;

public sealed class AzureAdOptions
{
    public const string SectionName = "AzureAd";

    public string TenantId { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
}
