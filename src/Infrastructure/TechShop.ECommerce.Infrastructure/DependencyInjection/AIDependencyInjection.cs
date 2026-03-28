using TechShop.ECommerce.Infrastructure.AI;

namespace TechShop.ECommerce.Infrastructure.DependencyInjection;

public static class AIDependencyInjection
{
    public static IServiceCollection AddAIInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<OllamaSettings>()
            .BindConfiguration(OllamaSettings.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddHttpClient<IEmbeddingProvider, OllamaEmbeddingProvider>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<OllamaSettings>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
        });

        services.AddHttpClient<IChatProvider, OllamaChatProvider>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<OllamaSettings>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
        });

        return services;
    }
}
