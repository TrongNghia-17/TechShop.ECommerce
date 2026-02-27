namespace TechShop.ECommerce.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseApiPipeline(this WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseMiddleware<CorrelationIdMiddleware>();

        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();
        app.UseCors("all");

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseRateLimiter();
        app.UseOutputCache();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.MapProductEndpoints();
        app.MapAuthEndpoints();
        app.MapCartEndpoints();
        app.MapOrderEndpoints();

        return app;
    }
}
