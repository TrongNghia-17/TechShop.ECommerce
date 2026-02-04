namespace TechShop.ECommerce.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseApiPipeline(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi("/openapi/{documentName}.json");

            app.MapScalarApiReference(options =>
            {
                options.OpenApiRoutePattern = "/openapi/{documentName}.json";

                options
                   .WithTheme(ScalarTheme.Kepler);
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
