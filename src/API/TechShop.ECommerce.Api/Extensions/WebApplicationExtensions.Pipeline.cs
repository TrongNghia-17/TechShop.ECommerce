namespace TechShop.ECommerce.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseApiPipeline(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", $"TechShop API {desc.GroupName}");
                }
            });
        }

        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();

        app.UseCors("all");
        app.UseAuthentication();
        app.UseAuthorization();

        // (prod note) forwarded headers should go before rate limiter
        // app.UseForwardedHeaders();

        app.UseRateLimiter();
        app.UseOutputCache();

        app.MapControllers();
        return app;
    }
}
