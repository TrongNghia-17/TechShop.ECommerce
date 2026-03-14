namespace TechShop.ECommerce.Application.Contracts.Storage;

public interface IFileStorage
{
    Task<string> UploadProductImageAsync(
        Guid productId,
        Stream stream,
        string contentType,
        string fileExtension,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        string blobName,
        CancellationToken cancellationToken = default);

    string? GetUrl(string? blobName);
}