using TechShop.ECommerce.Application.Common.Results;
using TechShop.ECommerce.Application.Contracts.AI;
using TechShop.ECommerce.Application.Contracts.Persistence;

namespace TechShop.ECommerce.Application.Features.Products.IngestProductVectors;

public sealed class IngestProductVectorsCommandHandler(
    IProductRepository productRepository,
    IEmbeddingProvider embeddingProvider,
    IPGVectorRepository vectorRepository)
    : IRequestHandler<IngestProductVectorsCommand, Result<int>>
{
    private const int BatchSize = 10;
    private const int BatchDelayMs = 500;

    public async Task<Result<int>> Handle(
        IngestProductVectorsCommand command,
        CancellationToken cancellationToken)
    {
        var products = await productRepository.GetAllForIngestionAsync(cancellationToken);

        if (products.Count == 0)
            return Result<int>.Success(0);

        var processed = 0;

        for (var i = 0; i < products.Count; i += BatchSize)
        {
            var batch = products
                .Skip(i)
                .Take(BatchSize)
                .ToList();

            var texts = batch
                .Select(product => $"{product.Name} {product.Summary} {product.Description}")
                .ToArray();

            var vectors = await embeddingProvider.EmbedBatchAsync(texts, cancellationToken);

            for (var j = 0; j < batch.Count; j++)
            {
                var product = batch[j];
                var embedding = vectors[j];

                await vectorRepository.InsertProductVectorAsync(product, embedding, cancellationToken);

                processed++;
            }

            await Task.Delay(BatchDelayMs, cancellationToken);
        }

        return Result<int>.Success(processed);
    }
}
