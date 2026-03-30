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

        // Lấy tất cả vector hiện có trong 1 lần query
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

        var products = await dbContext.ProductVectors
            .Include(productVector => productVector.Product)
            .ThenInclude(product => product.Category)
            .OrderBy(productVector => productVector.Embedding.CosineDistance(pgVector))
            .Take(topK)
            .Select(productVector => new ProductSearchModel(
                productVector.Product.Id.ToString(),
                productVector.Product.Name,
                productVector.Product.Description,
                productVector.Product.MainImageBlobName,
                productVector.Product.Price,
                new CategorySearchModel(productVector.Product.Category.Id.ToString(), productVector.Product.Category.Name),
                1.0f - (float)productVector.Embedding.CosineDistance(pgVector) // <-- Tính Score
            ))
            .ToListAsync(cancellationToken);

        return products.AsReadOnly();
    }

    public async Task<IReadOnlyList<ProductSearchModel>> SearchByKeywordAsync(string keyword, int topK = 5, CancellationToken cancellationToken = default)
    {
        var products = await dbContext.Products
            .Include(product => product.Category)
            .Where(product => EF.Functions.ILike(product.Name, $"%{keyword}%") ||
                              (product.Description != null && EF.Functions.ILike(product.Description, $"%{keyword}%")))
            .Take(topK)
            .Select(product => new ProductSearchModel(
                product.Id.ToString(),
                product.Name,
                product.Description,
                product.MainImageBlobName,
                product.Price,
                new CategorySearchModel(product.Category.Id.ToString(), product.Category.Name),
                1.0f // Keyword match mặc định là 1.0
            ))
            .ToListAsync(cancellationToken);

        return products.AsReadOnly();
    }

    public async Task<IReadOnlyList<ProductSearchModel>> HybridSearchAsync(string query, float[] queryVector, int topK = 5, CancellationToken cancellationToken = default)
    {
        var pgVector = new Pgvector.Vector(queryVector);

        var products = await dbContext.ProductVectors
            .Include(productVector => productVector.Product)
            .ThenInclude(product => product.Category)
            .Where(productVector => 
                EF.Functions.ILike(productVector.Product.Name, $"%{query}%") ||
                EF.Functions.ILike(productVector.Product.Category.Name, $"%{query}%") ||
                (productVector.Product.Category.Description != null && EF.Functions.ILike(productVector.Product.Category.Description, $"%{query}%")) ||
                (productVector.Product.Description != null && EF.Functions.ILike(productVector.Product.Description, $"%{query}%")))
            .OrderBy(productVector => productVector.Embedding.CosineDistance(pgVector))
            .Take(topK)
            .Select(productVector => new ProductSearchModel(
                productVector.Product.Id.ToString(),
                productVector.Product.Name,
                productVector.Product.Description,
                productVector.Product.MainImageBlobName,
                productVector.Product.Price,
                new CategorySearchModel(productVector.Product.Category.Id.ToString(), productVector.Product.Category.Name),
                1.0f - (float)productVector.Embedding.CosineDistance(pgVector) // <-- Tính Score cho Hybrid
            ))
            .ToListAsync(cancellationToken);

        if (!products.Any())
        {
            return await SearchByVectorAsync(queryVector, topK, cancellationToken);
        }

        return products.AsReadOnly();
    }
}
