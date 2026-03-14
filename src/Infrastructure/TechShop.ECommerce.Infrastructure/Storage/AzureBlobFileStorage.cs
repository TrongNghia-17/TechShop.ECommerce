using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using TechShop.ECommerce.Application.Contracts.Storage;

namespace TechShop.ECommerce.Infrastructure.Storage;

public sealed class AzureBlobFileStorage(
    BlobServiceClient blobServiceClient,
    IOptions<AzureStorageOptions> options)
    : IFileStorage
{
    private readonly BlobServiceClient _blobServiceClient = blobServiceClient;
    private readonly AzureStorageOptions _options = options.Value;

    public async Task<string> UploadProductImageAsync(
        Guid productId,
        Stream stream,
        string contentType,
        string fileExtension,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (productId == Guid.Empty)
            throw new ArgumentException("Product id is required.", nameof(productId));

        if (string.IsNullOrWhiteSpace(contentType))
            throw new ArgumentException("Content type is required.", nameof(contentType));

        if (string.IsNullOrWhiteSpace(fileExtension))
            throw new ArgumentException("File extension is required.", nameof(fileExtension));

        var blobName = BuildProductImageBlobName(productId, fileExtension);

        var containerClient = _blobServiceClient.GetBlobContainerClient(
            _options.ProductImagesContainerName);

        await containerClient.CreateIfNotExistsAsync(
            publicAccessType: PublicAccessType.Blob,
            cancellationToken: cancellationToken);

        var blobClient = containerClient.GetBlobClient(blobName);

        if (stream.CanSeek)
            stream.Position = 0;

        var uploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = contentType
            }
        };

        await blobClient.UploadAsync(
            stream,
            uploadOptions,
            cancellationToken);

        return blobName;
    }

    public async Task DeleteAsync(
        string blobName,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(blobName))
            return;

        var containerClient = _blobServiceClient.GetBlobContainerClient(
            _options.ProductImagesContainerName);

        var blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.DeleteIfExistsAsync(
            DeleteSnapshotsOption.IncludeSnapshots,
            cancellationToken: cancellationToken);
    }

    public string? GetUrl(string? blobName)
    {
        if (string.IsNullOrWhiteSpace(blobName))
            return null;

        var containerClient = _blobServiceClient.GetBlobContainerClient(
            _options.ProductImagesContainerName);

        var blobClient = containerClient.GetBlobClient(blobName);

        return blobClient.Uri.AbsoluteUri;
    }

    private static string BuildProductImageBlobName(Guid productId, string fileExtension)
    {
        var extension = fileExtension.Trim().ToLowerInvariant();

        if (!extension.StartsWith('.'))
            extension = "." + extension;

        return $"products/{productId}/{Guid.NewGuid():N}{extension}";
    }
}
