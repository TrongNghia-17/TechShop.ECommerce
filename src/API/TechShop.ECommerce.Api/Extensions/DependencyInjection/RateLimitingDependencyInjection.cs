using TechShop.ECommerce.Api.Extensions.RateLimiting;

namespace TechShop.ECommerce.Api.Extensions.DependencyInjection;

public static class RateLimitingDependencyInjection
{
    public static IServiceCollection AddApiRateLimiting(
        this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.OnRejected = static async (context, cancellationToken) =>
            {
                context.HttpContext.Response.ContentType = "application/json";

                await context.HttpContext.Response.WriteAsJsonAsync(
                    new
                    {
                        title = "TooManyRequests",
                        status = StatusCodes.Status429TooManyRequests,
                        detail = "Too many requests. Please try again later."
                    },
                    cancellationToken);
            };

            options.AddPolicy(RateLimitPolicies.AuthFixed, static httpContext =>
            {
                var partitionKey = GetClientPartitionKey(httpContext);

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey,
                    static _ => CreateFixedWindowLimiter(
                        permitLimit: 10,
                        window: TimeSpan.FromMinutes(1)));
            });

            options.AddPolicy(RateLimitPolicies.ProductsReadSliding, static httpContext =>
            {
                var partitionKey = GetClientPartitionKey(httpContext);

                return RateLimitPartition.GetSlidingWindowLimiter(
                    partitionKey,
                    static _ => CreateSlidingWindowLimiter(
                        permitLimit: 120,
                        window: TimeSpan.FromMinutes(1),
                        segmentsPerWindow: 6));
            });

            options.AddPolicy(RateLimitPolicies.ProductsManagementFixed, static httpContext =>
            {
                var partitionKey = GetClientPartitionKey(httpContext);

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey,
                    static _ => CreateFixedWindowLimiter(
                        permitLimit: 30,
                        window: TimeSpan.FromMinutes(1)));
            });

            options.AddPolicy(RateLimitPolicies.FileUploadFixed, static httpContext =>
            {
                var partitionKey = GetClientPartitionKey(httpContext);

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey,
                    static _ => CreateFixedWindowLimiter(
                        permitLimit: 10,
                        window: TimeSpan.FromMinutes(1)));
            });

            options.AddPolicy(RateLimitPolicies.CartFixed, static httpContext =>
            {
                var key = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return RateLimitPartition.GetFixedWindowLimiter(
                    key,
                    static _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 30,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    });
            });

            options.AddPolicy(RateLimitPolicies.OrdersFixed, static httpContext =>
            {
                var key = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return RateLimitPartition.GetFixedWindowLimiter(
                    key,
                    static _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 20,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    });
            });

            options.AddPolicy(RateLimitPolicies.WebhookFixed, static httpContext =>
            {
                var key = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return RateLimitPartition.GetFixedWindowLimiter(
                    key,
                    static _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 60,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    });
            });
        });

        return services;
    }

    private static string GetClientPartitionKey(HttpContext httpContext)
    {
        return httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }

    private static FixedWindowRateLimiterOptions CreateFixedWindowLimiter(
        int permitLimit,
        TimeSpan window)
    {
        return new FixedWindowRateLimiterOptions
        {
            PermitLimit = permitLimit,
            Window = window,
            QueueLimit = 0,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
        };
    }

    private static SlidingWindowRateLimiterOptions CreateSlidingWindowLimiter(
        int permitLimit,
        TimeSpan window,
        int segmentsPerWindow)
    {
        return new SlidingWindowRateLimiterOptions
        {
            PermitLimit = permitLimit,
            Window = window,
            SegmentsPerWindow = segmentsPerWindow,
            QueueLimit = 0,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
        };
    }
}