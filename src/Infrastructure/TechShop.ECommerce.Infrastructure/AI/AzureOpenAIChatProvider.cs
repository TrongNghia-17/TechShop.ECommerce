using System.Net.Http.Json;
using TechShop.ECommerce.Application.Common.Configurations.AI;
using TechShop.ECommerce.Application.Common.Models.AI;

namespace TechShop.ECommerce.Infrastructure.AI;

public sealed class AzureOpenAIChatProvider(
    HttpClient httpClient,
    IOptions<AzureOpenAIOptions> options) : IChatProvider
{
    private readonly AzureOpenAIOptions _settings = options.Value;

    public async Task<string> ChatAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var requestBody = new
        {
            messages = new[]
            {
                new { role = "system", content = "You are a professional AI shopping assistant for TechShop E-Commerce. Help users find tech products, compare prices, and suggest related items. Use the provided product catalog context if available. Always answer politely, concisely, and keep your answers relevant to electronics and technology." },
                new { role = "user", content = prompt }
            },
            max_tokens = 500
        };

        // Azure URL Format: /openai/deployments/{deployment}/chat/completions?api-version={version}
        var url = $"openai/deployments/{_settings.DeploymentName}/chat/completions?api-version={_settings.ApiVersion}";

        var response = await httpClient.PostAsJsonAsync(url, requestBody, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ChatCompletionResponse>(cancellationToken);

        return result?.Choices.FirstOrDefault()?.Message.Content ?? string.Empty;
    }
}
