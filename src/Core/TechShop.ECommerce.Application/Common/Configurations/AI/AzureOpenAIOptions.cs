using System.ComponentModel.DataAnnotations;

namespace TechShop.ECommerce.Application.Common.Configurations.AI;

public sealed class AzureOpenAIOptions
{
    public const string SectionName = "AI:AzureOpenAI";

    [Required]
    public string ApiKey { get; set; } = string.Empty;

    [Required, Url]
    public string BaseUrl { get; set; } = string.Empty; // Example: https://{resource-name}.openai.azure.com/

    [Required]
    public string DeploymentName { get; set; } = string.Empty;

    [Required]
    public string ApiVersion { get; set; } = "2024-02-15-preview";

    [Required]
    public string EmbeddingDeploymentName { get; set; } = string.Empty;

    public int Dimensions { get; set; } = 1536;
}
