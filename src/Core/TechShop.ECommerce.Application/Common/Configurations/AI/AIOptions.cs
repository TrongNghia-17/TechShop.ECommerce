using System.ComponentModel.DataAnnotations;

namespace TechShop.ECommerce.Application.Common.Configurations.AI;

public sealed class AIOptions
{
    public const string SectionName = "AI";

    [Required(ErrorMessage = "Please specify a valid AI Provider ('Ollama' or 'OpenAI').")]
    public string Provider { get; set; } = "Ollama";
}
