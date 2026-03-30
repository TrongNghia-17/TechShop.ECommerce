using TechShop.ECommerce.Application.Common.Results;
using TechShop.ECommerce.Application.Common.Telemetry;

namespace TechShop.ECommerce.Application.Behaviors;

public class TracingPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var commandName = typeof(TRequest).Name;

        using var activity = TelemetryConfig.ActivitySource.StartActivity($"Command: {commandName}");

        activity?.SetTag("messaging.system", "mediatr");

        var response = await next(cancellationToken);

        if (response is Result result && result.IsFailure)
        {
            activity?.SetStatus(ActivityStatusCode.Error, "Business logic failed");

            if (result.Error != null)
            {
                activity?.SetTag("error.code", result.Error.Code);
            }
        }
        else
        {
            activity?.SetStatus(ActivityStatusCode.Ok);
        }

        return response;
    }
}
