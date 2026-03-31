using System.Net.Http.Json;
using System.Text.Json.Serialization;
using TechShop.ECommerce.Application.Common.Configurations.AI;

namespace TechShop.ECommerce.Infrastructure.AI;

public sealed class AzureOpenAIEmbeddingProvider(
    HttpClient httpClient,
    IOptions<AzureOpenAIOptions> options,
    ILogger<AzureOpenAIEmbeddingProvider> logger) : IEmbeddingProvider
{
    private readonly AzureOpenAIOptions _settings = options.Value;
    public int Dimensions => _settings.Dimensions;

    public async Task<float[]> EmbedAsync(string text, CancellationToken cancellationToken = default)
    {
        var result = await EmbedBatchAsync([text], cancellationToken);
        return result.FirstOrDefault() ?? [];
    }

    public async Task<IReadOnlyList<float[]>> EmbedBatchAsync(
        IEnumerable<string> texts,
        CancellationToken cancellationToken = default)
    {
        var textList = texts.ToList();
        if (textList.Count == 0) return [];

        var requestBody = new { input = textList };

        // Azure URL Format: openai/deployments/{deployment}/embeddings?api-version={version}
        var url = $"openai/deployments/{_settings.EmbeddingDeploymentName}/embeddings?api-version={_settings.ApiVersion}";

        var response = await httpClient.PostAsJsonAsync(url, requestBody, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogError("Azure OpenAI Embedding API failed: {Error}", error);
            throw new InvalidOperationException($"Azure OpenAI Embedding API failed: {error}");
        }

        var result = await response.Content.ReadFromJsonAsync<AzureEmbeddingResponse>(cancellationToken);

        return result?.Data
            .OrderBy(x => x.Index)
            .Select(x => x.Embedding.Select(v => (float)v).ToArray())
            .ToList() ?? [];
    }

    private sealed record AzureEmbeddingResponse(
        [property: JsonPropertyName("data")] List<AzureEmbeddingItem> Data
    );

    private sealed record AzureEmbeddingItem(
        [property: JsonPropertyName("index")] int Index,
        [property: JsonPropertyName("embedding")] double[] Embedding
    );
}
