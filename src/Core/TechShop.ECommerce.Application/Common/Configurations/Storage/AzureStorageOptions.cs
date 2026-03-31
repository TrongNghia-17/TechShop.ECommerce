using System.ComponentModel.DataAnnotations;

namespace TechShop.ECommerce.Application.Common.Configurations.Storage;

public sealed class AzureStorageOptions
{
    public const string SectionName = "AzureStorage";

    [Required]
    public string ConnectionString { get; init; } = default!;

    [Required]
    public string ProductImagesContainerName { get; init; } = default!;

    [Range(1, 1440)]
    public int ReadUrlExpiryMinutes { get; init; } = 30;
}
