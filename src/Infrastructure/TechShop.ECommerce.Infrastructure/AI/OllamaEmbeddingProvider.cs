using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TechShop.ECommerce.Infrastructure.AI;

public class OllamaEmbeddingProvider(HttpClient httpClient, IOptions<OllamaSettings> options) : IEmbeddingProvider
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly OllamaSettings _settings = options.Value;

    public int Dimensions => _settings.Dimensions;

    public async Task<float[]> EmbedAsync(string text, CancellationToken cancellationToken = default)
    {
        var embedding = await TryEmbedWithModernEndpointAsync(text, cancellationToken);

        if (embedding.Length > 0)
        {
            return embedding;
        }

        return await EmbedWithLegacyEndpointAsync(text, cancellationToken);
    }

    public async Task<IReadOnlyList<float[]>> EmbedBatchAsync(IEnumerable<string> texts, CancellationToken cancellationToken = default)
    {
        var tasks = texts.Select(text => EmbedAsync(text, cancellationToken));
        var results = await Task.WhenAll(tasks);
        
        return results.ToList();
    }

    private async Task<float[]> TryEmbedWithModernEndpointAsync(
        string text,
        CancellationToken cancellationToken)
    {
        var requestBody = new
        {
            model = _settings.EmbeddingModel,
            input = text
        };

        using var response = await httpClient.PostAsJsonAsync("/api/embed", requestBody, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return Array.Empty<float>();
        }

        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonSerializer.Deserialize<OllamaEmbedResponse>(responseJson, JsonOptions);

        var embedding = result?.Embeddings?.FirstOrDefault();
        if (embedding == null || embedding.Length == 0)
        {
            return Array.Empty<float>();
        }

        return embedding.Select(value => (float)value).ToArray();
    }

    private async Task<float[]> EmbedWithLegacyEndpointAsync(
        string text,
        CancellationToken cancellationToken)
    {
        var requestBody = new
        {
            model = _settings.EmbeddingModel,
            prompt = text
        };

        using var response = await httpClient.PostAsJsonAsync("/api/embeddings", requestBody, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonSerializer.Deserialize<OllamaEmbeddingResponse>(responseJson, JsonOptions);

        if (result?.Embedding == null || result.Embedding.Length == 0)
        {
            return Array.Empty<float>();
        }

        return result.Embedding.Select(value => (float)value).ToArray();
    }

    private sealed class OllamaEmbedResponse
    {
        [JsonPropertyName("embeddings")]
        public double[][]? Embeddings { get; set; }
    }

    private sealed class OllamaEmbeddingResponse
    {
        [JsonPropertyName("embedding")]
        public double[]? Embedding { get; set; }
    }
}
