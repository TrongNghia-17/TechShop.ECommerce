using System.Net.Http.Json;
using System.Text.Json.Serialization;
using TechShop.ECommerce.Application.Common.Configurations.AI;

namespace TechShop.ECommerce.Infrastructure.AI;

public sealed class OllamaEmbeddingProvider(
    HttpClient httpClient,
    IOptions<OllamaOptions> options,
    ILogger<OllamaEmbeddingProvider> logger) : IEmbeddingProvider
{
    private readonly OllamaOptions _settings = options.Value;

    public int Dimensions => _settings.Dimensions;

    public async Task<float[]> EmbedAsync(string text, CancellationToken cancellationToken = default)
    {
        var results = await EmbedBatchAsync([text], cancellationToken);
        return results.FirstOrDefault()
            ?? [];
    }

    public async Task<IReadOnlyList<float[]>> EmbedBatchAsync(IEnumerable<string> texts, CancellationToken cancellationToken = default)
    {
        var textList = texts.ToList();
        if (textList.Count == 0) return [];

        try
        {
            // Thử sử dụng modern endpoint (/api/embed) - hỗ trợ nạp theo lô chính chủ Ollama
            var modernResult = await TryEmbedModernAsync(textList, cancellationToken);
            if (modernResult.Count > 0) return modernResult;

            // FALLBACK: Nếu Ollama phiên bản cũ không có /api/embed, chuyển sang chạy từng cái một
            logger.LogWarning("Ollama modern endpoint not available. Falling back to sequential embedding (Legacy)");

            var legacyResults = new List<float[]>();
            foreach (var text in textList)
            {
                legacyResults.Add(await EmbedLegacyAsync(text, cancellationToken));
            }
            return legacyResults;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during Ollama embedding for {Count} texts", textList.Count);
            throw;
        }
    }

    private async Task<IReadOnlyList<float[]>> TryEmbedModernAsync(List<string> inputs, CancellationToken cancellationToken)
    {
        var request = new { model = _settings.EmbeddingModel, input = inputs };

        using var response = await httpClient.PostAsJsonAsync("/api/embed", request, cancellationToken);

        // Trả về danh sách trống nếu endpoint không tồn tại
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return [];

        response.EnsureSuccessStatusCode();

        var responseContainer = await response.Content.ReadFromJsonAsync<OllamaEmbedResponse>(cancellationToken);
        if (responseContainer?.Embeddings == null)
        {
            return [];
        }

        // Tách ra để dễ debug: Chuyển đổi từ double[][] (API trả về) sang List<float[]> (Hệ thống dùng)
        var floatEmbeddings = responseContainer.Embeddings
            .Select(doubleArray => doubleArray.Select(v => (float)v).ToArray())
            .ToList();

        return floatEmbeddings;
    }

    private async Task<float[]> EmbedLegacyAsync(string text, CancellationToken cancellationToken)
    {
        var request = new { model = _settings.EmbeddingModel, prompt = text };

        using var response = await httpClient.PostAsJsonAsync("/api/embeddings", request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseContainer = await response.Content.ReadFromJsonAsync<OllamaEmbeddingResponse>(cancellationToken);
        if (responseContainer?.Embedding == null)
        {
            return [];
        }

        // Tách ra để dễ debug: Chuyển đổi mảng double[] sang float[]
        var floatArray = responseContainer.Embedding
            .Select(v => (float)v)
            .ToArray();

        return floatArray;
    }

    // Records: Cú pháp hiện đại, tối giản và bất biến (Immutable)
    private record OllamaEmbedResponse(
        [property: JsonPropertyName("embeddings")] double[][]? Embeddings
    );

    private record OllamaEmbeddingResponse(
        [property: JsonPropertyName("embedding")] double[]? Embedding
    );
}
