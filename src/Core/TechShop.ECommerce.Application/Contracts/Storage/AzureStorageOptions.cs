namespace TechShop.ECommerce.Application.Contracts.Storage;

public sealed class AzureStorageOptions
{
    [Required]
    public string ConnectionString { get; set; } = default!;

    [Required]
    public string ContainerName { get; set; } = default!;
}
