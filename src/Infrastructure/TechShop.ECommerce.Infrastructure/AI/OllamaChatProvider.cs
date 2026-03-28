using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TechShop.ECommerce.Infrastructure.AI;

public sealed class OllamaChatProvider(
    HttpClient httpClient, 
    IOptions<OllamaSettings> options, 
    ILogger<OllamaChatProvider> logger) : IChatProvider
{
    private readonly HttpClient _httpClient = SetupClient(httpClient);
    private readonly OllamaSettings _settings = options.Value;

    private static HttpClient SetupClient(HttpClient client)
    {
        client.Timeout = TimeSpan.FromMinutes(5);
        return client;
    }

    public async Task<string> ChatAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var requestBody = new
        {
            model = _settings.ChatModel,
            prompt = prompt,
            stream = false,
            options = new
            {
                num_predict = _settings.MaxTokens,
                temperature = _settings.Temperature
            }
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/generate", requestBody, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<OllamaChatResponse>(responseJson);

            return result?.Response ?? string.Empty;
        }
        catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            logger.LogError(ex, "Ollama API Timed Out.");
            return "[Error: Response Timeout] AI provider timeout.";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ollama API Exception.");
            return "[Error: Provider Error] AI provider error.";
        }
    }

    private class OllamaChatResponse
    {
        [JsonPropertyName("response")]
        public string? Response { get; set; }
    }
}
