using TechShop.ECommerce.Api.Endpoints;
using TechShop.ECommerce.Api.Middleware;

namespace TechShop.ECommerce.Api.Extensions.Pipeline;

public static class ApiPipelineExtensions
{
    private const string FrontendCorsPolicy = "Frontend";

    public static WebApplication UseApiPipeline(this WebApplication app)
    {
        app.UseExceptionHandler();

        if (!app.Environment.IsDevelopment())
        {
            app.UseHsts();
        }

        app.UseResponseCompression();
        app.UseMiddleware<SecurityHeadersMiddleware>();
        app.UseMiddleware<CorrelationIdMiddleware>();

        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();
        app.UseCors(FrontendCorsPolicy);

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
            app.MapScalarApiReference(options =>
            {
                options.AddAuthorizationCodeFlow("oauth2", flow => 
                {
                    flow.ClientId = app.Configuration["AzureAd:ClientId"];
                    flow.Pkce = Scalar.AspNetCore.Pkce.Sha256; 
                    flow.CredentialsLocation = Scalar.AspNetCore.CredentialsLocation.Body;
                });
            });
        }

        app.MapStripeWebhookEndpoints();
        app.MapProductEndpoints();
        app.MapSemanticEndpoints();
        app.MapCartEndpoints();
        app.MapOrderEndpoints();
        app.MapTechShopHealthChecks();

        return app;
    }
}
