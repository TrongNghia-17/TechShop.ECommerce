using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using TechShop.ECommerce.Application.Common.Configurations.AI;

namespace TechShop.ECommerce.Infrastructure.AI;

public sealed class OpenAIEmbeddingProvider(
    HttpClient httpClient,
    IOptions<OpenAIOptions> options,
    ILogger<OpenAIEmbeddingProvider> logger) : IEmbeddingProvider
{
    private readonly OpenAIOptions _settings = options.Value;
    public int Dimensions => _settings.Dimensions;

    public async Task<float[]> EmbedAsync(string text, CancellationToken cancellationToken = default)
    {
        var results = await EmbedBatchAsync([text], cancellationToken);
        return results.FirstOrDefault() ?? [];
    }

    public async Task<IReadOnlyList<float[]>> EmbedBatchAsync(
        IEnumerable<string> texts,
        CancellationToken cancellationToken = default)
    {
        var textList = texts.ToList();

        if (textList.Count == 0)
        {
            return [];
        }

        try
        {
            var requestBody = new
            {
                input = textList,
                model = _settings.EmbeddingModel
            };

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _settings.ApiKey);

            using var response = await httpClient.PostAsJsonAsync(
                "https://api.openai.com/v1/embeddings",
                requestBody,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<OpenAIEmbeddingResponse>(cancellationToken);

            if (result?.Data == null)
            {
                return [];
            }

            return result.Data
                .Select(x => x.Embedding.Select(v => (float)v).ToArray())
                .ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while creating embeddings with OpenAI");
            throw;
        }
    }

    private sealed record OpenAIEmbeddingResponse(
        [property: JsonPropertyName("data")] List<OpenAIEmbeddingItem> Data
    );

    private sealed record OpenAIEmbeddingItem(
        [property: JsonPropertyName("embedding")] double[] Embedding
    );
}
