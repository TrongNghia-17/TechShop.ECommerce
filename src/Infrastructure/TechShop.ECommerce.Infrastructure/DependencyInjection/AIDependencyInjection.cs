using System.Net.Http.Headers;
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

        switch (aiProvider.ToLower())
        {
            case "openai":
                services.AddOptions<OpenAIOptions>()
                    .BindConfiguration(OpenAIOptions.SectionName)
                    .ValidateDataAnnotations()
                    .ValidateOnStart();

                services.AddHttpClient<IEmbeddingProvider, OpenAIEmbeddingProvider>((sp, client) =>
                {
                    var opt = sp.GetRequiredService<IOptions<OpenAIOptions>>().Value;
                    client.BaseAddress = new Uri(opt.BaseUrl);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", opt.ApiKey);
                    client.Timeout = TimeSpan.FromMinutes(2);
                });

                services.AddHttpClient<IChatProvider, OpenAIChatProvider>((sp, client) =>
                {
                    var opt = sp.GetRequiredService<IOptions<OpenAIOptions>>().Value;
                    client.BaseAddress = new Uri(opt.BaseUrl);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", opt.ApiKey);
                    client.Timeout = TimeSpan.FromMinutes(2);
                });
                break;

            case "azureopenai":
                services.AddOptions<AzureOpenAIOptions>()
                    .BindConfiguration(AzureOpenAIOptions.SectionName)
                    .ValidateDataAnnotations()
                    .ValidateOnStart();

                services.AddHttpClient<IEmbeddingProvider, AzureOpenAIEmbeddingProvider>((sp, client) =>
                {
                    var opt = sp.GetRequiredService<IOptions<AzureOpenAIOptions>>().Value;
                    client.BaseAddress = new Uri(opt.BaseUrl);
                    client.DefaultRequestHeaders.Add("api-key", opt.ApiKey);
                    client.Timeout = TimeSpan.FromMinutes(2);
                });

                services.AddHttpClient<IChatProvider, AzureOpenAIChatProvider>((sp, client) =>
                {
                    var opt = sp.GetRequiredService<IOptions<AzureOpenAIOptions>>().Value;
                    client.BaseAddress = new Uri(opt.BaseUrl);
                    client.DefaultRequestHeaders.Add("api-key", opt.ApiKey);
                    client.Timeout = TimeSpan.FromMinutes(2);
                });
                break;

            case "ollama":
            default:
                services.AddOptions<OllamaOptions>()
                    .BindConfiguration(OllamaOptions.SectionName)
                    .ValidateDataAnnotations()
                    .ValidateOnStart();

                services.AddHttpClient<IEmbeddingProvider, OllamaEmbeddingProvider>((sp, client) =>
                {
                    var opt = sp.GetRequiredService<IOptions<OllamaOptions>>().Value;
                    client.BaseAddress = new Uri(opt.BaseUrl);
                    client.Timeout = TimeSpan.FromMinutes(5);
                });

                services.AddHttpClient<IChatProvider, OllamaChatProvider>((sp, client) =>
                {
                    var opt = sp.GetRequiredService<IOptions<OllamaOptions>>().Value;
                    client.BaseAddress = new Uri(opt.BaseUrl);
                    client.Timeout = TimeSpan.FromMinutes(5);
                });
                break;
        }

        return services;
    }
}
