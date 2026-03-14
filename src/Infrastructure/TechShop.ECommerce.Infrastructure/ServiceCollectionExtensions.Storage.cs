using Azure.Storage.Blobs;
using TechShop.ECommerce.Application.Contracts.Storage;
using TechShop.ECommerce.Infrastructure.Storage;

namespace TechShop.ECommerce.Infrastructure;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddStorageServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<AzureStorageOptions>()
            .BindConfiguration(AzureStorageOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton(serviceProvider =>
        {
            var options = serviceProvider
                .GetRequiredService<IOptions<AzureStorageOptions>>()
                .Value;

            return new BlobServiceClient(options.ConnectionString);
        });

        services.AddScoped<IFileStorage, AzureBlobFileStorage>();

        return services;
    }
}