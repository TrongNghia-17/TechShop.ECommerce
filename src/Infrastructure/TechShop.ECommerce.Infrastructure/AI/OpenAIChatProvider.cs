using System.Net.Http.Json;
using TechShop.ECommerce.Application.Common.Configurations.AI;
using TechShop.ECommerce.Application.Common.Models.AI;

namespace TechShop.ECommerce.Infrastructure.AI;

public sealed class OpenAIChatProvider(
    HttpClient httpClient,
    IOptions<OpenAIOptions> options) : IChatProvider
{
    private readonly OpenAIOptions _settings = options.Value;

    public async Task<string> ChatAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var requestBody = new
        {
            model = _settings.ChatModel,
            messages = new[]
            {
                new { role = "system", content = "You are a professional AI shopping assistant for TechShop E-Commerce. Help users find tech products, compare prices, and suggest related items. Use the provided product catalog context if available. Always answer politely, concisely, and keep your answers relevant to electronics and technology." },
                new { role = "user", content = prompt }
            },
            max_tokens = 500,
            temperature = _settings.Temperature
        };

        var response = await httpClient.PostAsJsonAsync("chat/completions", requestBody, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ChatCompletionResponse>(cancellationToken);

        return result?.Choices.FirstOrDefault()?.Message.Content ?? string.Empty;
    }
}
