using Microsoft.Extensions.Options;
using TechShop.ECommerce.Application.Common.Configurations.AI;
using TechShop.ECommerce.Application.Common.Results;
using TechShop.ECommerce.Application.Contracts.AI;
using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Domain.Entities.Catalogs;

namespace TechShop.ECommerce.Application.Features.Products.IngestProductVectors;

public sealed class IngestProductVectorsCommandHandler(
    IProductRepository productRepository,
    IEmbeddingProvider embeddingProvider,
    IPGVectorRepository vectorRepository,
    IOptions<IngestionOptions> ingestionOptions)
    : IRequestHandler<IngestProductVectorsCommand, Result<int>>
{
    private readonly IngestionOptions _settings = ingestionOptions.Value;

    public async Task<Result<int>> Handle(
        IngestProductVectorsCommand command,
        CancellationToken cancellationToken)
    {
        var products = await productRepository.GetAllForIngestionAsync(cancellationToken);

        if (products.Count == 0)
            return Result<int>.Success(0);

        var totalProcessed = 0;

        // Xử lý theo từng lô (Batching) để tối ưu RAM và CPU
        for (var i = 0; i < products.Count; i += _settings.BatchSize)
        {
            var currentBatch = products.Skip(i).Take(_settings.BatchSize).ToList();

            // 1. Chuẩn bị nội dung văn bản để AI học
            var texts = currentBatch.Select(PrepareEmbeddingText).ToArray();

            // 2. Chuyển đổi hàng loạt sang Vectors (Ollama Batch Embed)
            var embeddings = await embeddingProvider.EmbedBatchAsync(texts, cancellationToken);

            // 3. Chuẩn bị dữ liệu để lưu vào DB theo lô
            var upsertData = currentBatch
                .Select((product, index) => (product, embeddings[index]))
                .ToList();

            // 4. Lưu toàn bộ lô vào Postgres (Chỉ 1 lần SaveChanges)
            await vectorRepository.UpsertProductVectorsAsync(upsertData, cancellationToken);

            totalProcessed += currentBatch.Count;

            // 5. Nghỉ một chút giữa các đợt để không làm quá tải CPU/RAM của hệ thống AI
            if (i + _settings.BatchSize < products.Count) // Chỉ nghỉ nếu vẫn còn lô tiếp theo
            {
                await Task.Delay(_settings.BatchDelayMs, cancellationToken);
            }
        }

        return Result<int>.Success(totalProcessed);
    }

    private static string PrepareEmbeddingText(Product product)
    {
        // Nội dung càng chi tiết, AI tìm kiếm càng thông minh
        return $"Category: {product.Category.Name} ({product.Category.Description}). " +
               $"Product: {product.Name}. " +
               $"Summary: {product.Summary}. " +
               $"Description: {product.Description}";
    }
}
