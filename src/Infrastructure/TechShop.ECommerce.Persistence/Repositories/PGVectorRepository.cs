using Pgvector.EntityFrameworkCore;
using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Application.Features.Products.Shared;
using TechShop.ECommerce.Persistence.Context;

namespace TechShop.ECommerce.Persistence.Repositories;

public class PGVectorRepository(TechShopDbContext dbContext) : IPGVectorRepository
{
    public async Task InsertProductVectorAsync(Product product, float[] embeddings, CancellationToken cancellationToken = default)
    {
        await UpsertProductVectorsAsync([(product, embeddings)], cancellationToken);
    }

    public async Task UpsertProductVectorsAsync(IEnumerable<(Product Product, float[] Embedding)> batch, CancellationToken cancellationToken = default)
    {
        var productIds = batch.Select(b => b.Product.Id).ToList();

        var existingVectors = await dbContext.ProductVectors
            .Where(pv => productIds.Contains(pv.ProductId))
            .ToDictionaryAsync(pv => pv.ProductId, cancellationToken);

        foreach (var (product, embedding) in batch)
        {
            if (existingVectors.TryGetValue(product.Id, out var existingVector))
            {
                existingVector.UpdateEmbedding(embedding);
            }
            else
            {
                var newVector = new ProductVector(product.Id, embedding);
                await dbContext.ProductVectors.AddAsync(newVector, cancellationToken);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProductSearchModel>> SearchByVectorAsync(float[] queryVector, int topK = 5, CancellationToken cancellationToken = default)
    {
        var pgVector = new Pgvector.Vector(queryVector);

        // BẮT BUỘC: Select phải viết tường minh để EF Core dịch được sang SQL
        var results = await dbContext.ProductVectors
            .Include(pv => pv.Product).ThenInclude(p => p.Category)
            .OrderBy(pv => pv.Embedding.CosineDistance(pgVector))
            .Take(topK)
            .Select(pv => new ProductSearchModel(
                pv.Product.Id.ToString(),
                pv.Product.Name,
                pv.Product.Description,
                pv.Product.MainImageBlobName,
                pv.Product.Price,
                new CategorySearchModel(pv.Product.Category.Id.ToString(), pv.Product.Category.Name),
                1.0f - (float)pv.Embedding.CosineDistance(pgVector)
            ))
            .ToListAsync(cancellationToken);

        return results.AsReadOnly();
    }

    public async Task<IReadOnlyList<ProductSearchModel>> HybridSearchAsync(string query, float[] queryVector, int topK = 5, CancellationToken cancellationToken = default)
    {
        var pgVector = new Pgvector.Vector(queryVector);

        var results = await dbContext.ProductVectors
            .Include(pv => pv.Product).ThenInclude(p => p.Category)
            .Where(pv =>
                EF.Functions.ILike(pv.Product.Name, $"%{query}%") ||
                EF.Functions.ILike(pv.Product.Category.Name, $"%{query}%") ||
                (pv.Product.Category.Description != null && EF.Functions.ILike(pv.Product.Category.Description, $"%{query}%")) ||
                (pv.Product.Description != null && EF.Functions.ILike(pv.Product.Description, $"%{query}%")))
            .OrderBy(pv => pv.Embedding.CosineDistance(pgVector))
            .Take(topK)
            .Select(pv => new ProductSearchModel(
                pv.Product.Id.ToString(),
                pv.Product.Name,
                pv.Product.Description,
                pv.Product.MainImageBlobName,
                pv.Product.Price,
                new CategorySearchModel(pv.Product.Category.Id.ToString(), pv.Product.Category.Name),
                1.0f - (float)pv.Embedding.CosineDistance(pgVector)
            ))
            .ToListAsync(cancellationToken);

        if (results.Count == 0)
        {
            return await SearchByVectorAsync(queryVector, topK, cancellationToken);
        }

        return results.AsReadOnly();
    }
}
