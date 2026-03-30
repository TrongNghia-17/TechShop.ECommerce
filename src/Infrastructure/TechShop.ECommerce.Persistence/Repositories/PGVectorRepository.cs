using Pgvector.EntityFrameworkCore;
using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Application.Features.Products.Shared;
using TechShop.ECommerce.Persistence.Context;

namespace TechShop.ECommerce.Persistence.Repositories;

public class PGVectorRepository(TechShopDbContext dbContext) : IPGVectorRepository
{
    public async Task InsertProductVectorAsync(Product product, float[] embeddings, CancellationToken cancellationToken = default)
    {
        var productVector = new ProductVector(product.Id, embeddings);
        dbContext.ProductVectors.Add(productVector);

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
                new CategorySearchModel(productVector.Product.Category.Id.ToString(), productVector.Product.Category.Name)
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
                new CategorySearchModel(product.Category.Id.ToString(), product.Category.Name)
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
            .Where(productVector => EF.Functions.ILike(productVector.Product.Name, $"%{query}%") ||
                                    (productVector.Product.Description != null && EF.Functions.ILike(productVector.Product.Description, $"%{query}%")))
            .OrderBy(productVector => productVector.Embedding.CosineDistance(pgVector))
            .Take(topK)
            .Select(productVector => new ProductSearchModel(
                productVector.Product.Id.ToString(),
                productVector.Product.Name,
                productVector.Product.Description,
                productVector.Product.MainImageBlobName,
                productVector.Product.Price,
                new CategorySearchModel(productVector.Product.Category.Id.ToString(), productVector.Product.Category.Name)
            ))
            .ToListAsync(cancellationToken);

        if (!products.Any())
        {
            return await SearchByVectorAsync(queryVector, topK, cancellationToken);
        }

        return products.AsReadOnly();
    }
}
