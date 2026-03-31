using Azure.Storage.Blobs;
using TechShop.ECommerce.Application.Common.Configurations.Storage;
using TechShop.ECommerce.Application.Contracts.Storage;
using TechShop.ECommerce.Infrastructure.Storage;

namespace TechShop.ECommerce.Infrastructure.DependencyInjection;

public static class StorageDependencyInjection
{
    public static IServiceCollection AddStorageInfrastructure(this IServiceCollection services)
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