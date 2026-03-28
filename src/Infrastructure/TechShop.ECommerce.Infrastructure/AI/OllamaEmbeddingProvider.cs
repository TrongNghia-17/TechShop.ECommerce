using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TechShop.ECommerce.Infrastructure.AI;

public class OllamaEmbeddingProvider(HttpClient httpClient, IOptions<OllamaSettings> options) : IEmbeddingProvider
{
    private readonly OllamaSettings _settings = options.Value;

    public int Dimensions => _settings.Dimensions;

    public async Task<float[]> EmbedAsync(string text, CancellationToken cancellationToken = default)
    {
        var requestBody = new
        {
            model = _settings.EmbeddingModel,
            prompt = text
        };

        var response = await httpClient.PostAsJsonAsync("/api/embeddings", requestBody, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonSerializer.Deserialize<OllamaEmbeddingResponse>(responseJson);

        if (result?.Embedding == null || result.Embedding.Length == 0)
        {
            return Array.Empty<float>();
        }

        return result.Embedding.Select(e => (float)e).ToArray();
    }

    public async Task<IReadOnlyList<float[]>> EmbedBatchAsync(IEnumerable<string> texts, CancellationToken cancellationToken = default)
    {
        var tasks = texts.Select(text => EmbedAsync(text, cancellationToken));
        var results = await Task.WhenAll(tasks);
        
        return results.ToList();
    }

    private class OllamaEmbeddingResponse
    {
        [JsonPropertyName("embedding")]
        public double[]? Embedding { get; set; }
    }
}
