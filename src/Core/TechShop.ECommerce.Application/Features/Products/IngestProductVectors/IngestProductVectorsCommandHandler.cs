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

        for (var i = 0; i < products.Count; i += _settings.BatchSize)
        {
            var currentBatch = products.Skip(i).Take(_settings.BatchSize).ToList();

            var texts = currentBatch.Select(PrepareEmbeddingText).ToArray();

            var embeddings = await embeddingProvider.EmbedBatchAsync(texts, cancellationToken);

            var upsertData = currentBatch
                .Select((product, index) => (product, embeddings[index]))
                .ToList();

            await vectorRepository.UpsertProductVectorsAsync(upsertData, cancellationToken);

            totalProcessed += currentBatch.Count;

            if (i + _settings.BatchSize < products.Count)
            {
                await Task.Delay(_settings.BatchDelayMs, cancellationToken);
            }
        }

        return Result<int>.Success(totalProcessed);
    }

    private static string PrepareEmbeddingText(Product product)
    {
        return $"Category: {product.Category.Name} ({product.Category.Description}). " +
               $"Product: {product.Name}. " +
               $"Summary: {product.Summary}. " +
               $"Description: {product.Description}";
    }
}
