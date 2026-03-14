namespace TechShop.ECommerce.Application.Contracts.Storage;

public sealed class AzureStorageOptions
{
    public const string SectionName = "AzureStorage";

    [Required]
    public string ConnectionString { get; init; } = default!;

    [Required]
    public string ProductImagesContainerName { get; init; } = default!;
}
