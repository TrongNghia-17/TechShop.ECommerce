using System.ComponentModel.DataAnnotations;

namespace TechShop.ECommerce.Infrastructure.AI;

public class OllamaSettings
{
    public const string SectionName = "Ollama";

    [Required, Url]
    public string BaseUrl { get; set; } = "http://localhost:11434";

    [Required]
    public string EmbeddingModel { get; set; } = "nomic-embed-text";

    [Required]
    public string ChatModel { get; set; } = "tinyllama";

    [Range(1, 10000)]
    public int Dimensions { get; set; } = 768;

    [Range(1, 100000)]
    public int MaxTokens { get; set; } = 2048;

    [Range(0.0, 2.0)]
    public double Temperature { get; set; } = 0.7;
}
