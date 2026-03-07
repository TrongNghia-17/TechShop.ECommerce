using Hangfire;

namespace TechShop.ECommerce.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseApiPipeline(this WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseResponseCompression();
        app.UseMiddleware<CorrelationIdMiddleware>();

        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();
        app.UseCors("all");

        app.UseAuthentication();
        app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.UseHangfireDashboard("/hangfire");
        }

        app.UseRateLimiter();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.MapStripeWebhookEndpoints();
        app.MapProductEndpoints();
        app.MapAuthEndpoints();
        app.MapCartEndpoints();
        app.MapOrderEndpoints();

        return app;
    }
}
