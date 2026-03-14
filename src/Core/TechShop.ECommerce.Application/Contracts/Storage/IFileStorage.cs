namespace TechShop.ECommerce.Application.Contracts.Storage;

public interface IFileStorage
{
    Task<string> UploadProductImageAsync(
        Guid productId,
        Stream inputImageStream,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        string blobName,
        CancellationToken cancellationToken = default);

    string? GetUrl(string? blobName);
}