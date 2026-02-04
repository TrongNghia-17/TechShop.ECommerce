namespace TechShop.ECommerce.Api.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenApiWithVersioning(this IServiceCollection services)
    {
        void ApplyCommonInfo(OpenApiDocument document, string docName)
        {
            document.Info = new OpenApiInfo
            {
                Title = "TechShop API",
                Version = docName,
                Description = "TechShop E-Commerce API",
                TermsOfService = new Uri("https://your-domain.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "TechShop Team",
                    Email = "support@your-domain.com",
                    Url = new Uri("https://your-domain.com")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            };

            document.Servers ??= [];
            document.Servers.Add(new OpenApiServer
            {
                Url = "https://localhost:7125",
                Description = "Local Dev"
            });
        }

        services.AddOpenApi("v1", options =>
        {
            options.AddDocumentTransformer((document, context, ct) =>
            {
                ApplyCommonInfo(document, "v1");
                return Task.CompletedTask;
            });
        });

        services.AddOpenApi("v2", options =>
        {
            options.AddDocumentTransformer((document, context, ct) =>
            {
                ApplyCommonInfo(document, "v2");
                return Task.CompletedTask;
            });
        });

        return services;
    }
}
