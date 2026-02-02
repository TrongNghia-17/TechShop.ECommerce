namespace TechShop.ECommerce.Api.Swagger;

public sealed class ConfigureSwaggerOptions(
    IApiVersionDescriptionProvider provider)
    : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var desc in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(desc.GroupName, new OpenApiInfo
            {
                Title = "TechShop API",
                Version = desc.GroupName
            });
        }
    }
}
