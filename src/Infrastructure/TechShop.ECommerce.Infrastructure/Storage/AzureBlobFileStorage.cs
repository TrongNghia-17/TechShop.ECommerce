using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using TechShop.ECommerce.Application.Contracts.Storage;

namespace TechShop.ECommerce.Infrastructure.Storage;

public sealed class AzureBlobFileStorage(
    BlobServiceClient blobServiceClient,
    IOptions<AzureStorageOptions> options)
    : IFileStorage
{
    private const int MaxWidth = 800;
    private const int MaxHeight = 800;
    private const int JpegQuality = 85;
    private const string OutputContentType = "image/jpeg";
    private const string OutputFileExtension = ".jpg";

    private readonly BlobServiceClient _blobServiceClient = blobServiceClient;
    private readonly AzureStorageOptions _options = options.Value;

    public async Task<string> UploadProductImageAsync(
        Guid productId,
        Stream inputImageStream,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(inputImageStream);

        if (productId == Guid.Empty)
            throw new ArgumentException("Product id is required.", nameof(productId));

        var blobName = BuildProductImageBlobName(productId, OutputFileExtension);

        await using var processedImageStream = await ResizeAndConvertToJpegAsync(
            inputImageStream,
            cancellationToken);

        var containerClient = _blobServiceClient.GetBlobContainerClient(
            _options.ProductImagesContainerName);

        await containerClient.CreateIfNotExistsAsync(
            cancellationToken: cancellationToken);

        var blobClient = containerClient.GetBlobClient(blobName);

        var uploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = OutputContentType
            }
        };

        await blobClient.UploadAsync(
            processedImageStream,
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

    public string? GetReadUrl(
        string? blobName,
        TimeSpan? lifetime = null)
    {
        if (string.IsNullOrWhiteSpace(blobName))
            return null;

        var containerClient = _blobServiceClient.GetBlobContainerClient(
            _options.ProductImagesContainerName);

        var blobClient = containerClient.GetBlobClient(blobName);

        if (!blobClient.CanGenerateSasUri)
            return blobClient.Uri.AbsoluteUri;

        var effectiveLifetime = lifetime ?? TimeSpan.FromMinutes(_options.ReadUrlExpiryMinutes);

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerClient.Name,
            BlobName = blobClient.Name,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.Add(effectiveLifetime)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        var sasUri = blobClient.GenerateSasUri(sasBuilder);

        return sasUri.AbsoluteUri;
    }

    private static string BuildProductImageBlobName(
        Guid productId,
        string outputFileExtension)
    {
        var normalizedExtension = outputFileExtension.Trim().ToLowerInvariant();

        if (!normalizedExtension.StartsWith('.'))
            normalizedExtension = "." + normalizedExtension;

        return $"products/{productId}/{Guid.NewGuid():N}{normalizedExtension}";
    }

    private static async Task<MemoryStream> ResizeAndConvertToJpegAsync(
        Stream inputImageStream,
        CancellationToken cancellationToken)
    {
        if (inputImageStream.CanSeek)
            inputImageStream.Position = 0;

        using var image = await Image.LoadAsync(
            inputImageStream,
            cancellationToken);

        image.Mutate(static x => x.Resize(new ResizeOptions
        {
            Size = new Size(MaxWidth, MaxHeight),
            Mode = ResizeMode.Max
        }));

        var outputStream = new MemoryStream();

        await image.SaveAsJpegAsync(
            outputStream,
            new JpegEncoder
            {
                Quality = JpegQuality
            },
            cancellationToken);

        outputStream.Position = 0;

        return outputStream;
    }
}
