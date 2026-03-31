using System.ComponentModel.DataAnnotations;

namespace TechShop.ECommerce.Application.Common.Configurations.AI;

public sealed class OpenAIOptions
{
    public const string SectionName = "AI:OpenAI";

    [Required]
    public string ApiKey { get; set; } = string.Empty;

    [Required, Url]
    public string BaseUrl { get; set; } = "https://api.openai.com/v1/";

    [Required]
    public string EmbeddingModel { get; set; } = "text-embedding-3-small";

    [Range(1, 4096)]
    public int Dimensions { get; set; } = 1536;

    [Required]
    public string ChatModel { get; set; } = "gpt-4o-mini";
}