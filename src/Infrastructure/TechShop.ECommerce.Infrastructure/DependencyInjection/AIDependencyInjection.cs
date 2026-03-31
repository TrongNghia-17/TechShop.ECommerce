using TechShop.ECommerce.Application.Common.Configurations.AI;
using TechShop.ECommerce.Infrastructure.AI;

namespace TechShop.ECommerce.Infrastructure.DependencyInjection;

public static class AIDependencyInjection
{
    public static IServiceCollection AddAIInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var aiSection = configuration.GetSection(AIOptions.SectionName);
        services.Configure<AIOptions>(aiSection);

        var aiSettings = aiSection.Get<AIOptions>();
        var aiProvider = aiSettings?.Provider ?? "Ollama";

        if (aiProvider.Equals("OpenAI", StringComparison.OrdinalIgnoreCase))
        {
            services.AddOptions<OpenAIOptions>()
                .BindConfiguration(OpenAIOptions.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddHttpClient<IEmbeddingProvider, OpenAIEmbeddingProvider>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<OpenAIOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
                client.Timeout = TimeSpan.FromMinutes(2);
            });
        }
        else
        {
            services.AddOptions<OllamaOptions>()
                .BindConfiguration(OllamaOptions.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddHttpClient<IEmbeddingProvider, OllamaEmbeddingProvider>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<OllamaOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
                client.Timeout = TimeSpan.FromMinutes(5);
            });

            services.AddHttpClient<IChatProvider, OllamaChatProvider>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<OllamaOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
                client.Timeout = TimeSpan.FromMinutes(5);
            });
        }

        return services;
    }
}
