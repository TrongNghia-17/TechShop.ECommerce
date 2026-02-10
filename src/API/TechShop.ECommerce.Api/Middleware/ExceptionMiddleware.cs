namespace TechShop.ECommerce.Api.Middleware;

public class ExceptionMiddleware(
    RequestDelegate next,
    ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        if (context.Response.HasStarted)
        {
            logger.LogWarning(
                ex,
                "Response already started. Path: {Path}",
                context.Request.Path
            );

            ExceptionDispatchInfo.Capture(ex).Throw();
        }

        CustomProblemDetails problem;
        LogLevel logLevel;

        switch (ex)
        {
            case BadRequestException badRequest:
                problem = new CustomProblemDetails
                {
                    Title = "Bad request",
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://httpstatuses.com/400",
                    Detail = badRequest.Message,
                    Instance = context.Request.Path,
                    ErrorCode = "VALIDATION_ERROR",
                    Errors = badRequest.ValidationErrors
                };
                logLevel = LogLevel.Warning;
                break;

            case NotFoundException notFound:
                problem = new CustomProblemDetails
                {
                    Title = "Resource not found",
                    Status = StatusCodes.Status404NotFound,
                    Type = "https://httpstatuses.com/404",
                    Detail = notFound.Message,
                    Instance = context.Request.Path,
                    ErrorCode = "RESOURCE_NOT_FOUND"
                };
                logLevel = LogLevel.Information;
                break;

            case ConcurrencyException concurrency:
                problem = new CustomProblemDetails
                {
                    Title = "Concurrency conflict",
                    Status = StatusCodes.Status409Conflict,
                    Type = "https://httpstatuses.com/409",
                    Detail = concurrency.Message,
                    Instance = context.Request.Path,
                    ErrorCode = "CONCURRENCY_CONFLICT"
                };
                logLevel = LogLevel.Warning;
                break;

            default:
                problem = new CustomProblemDetails
                {
                    Title = "An unexpected error occurred",
                    Status = StatusCodes.Status500InternalServerError,
                    Type = "https://httpstatuses.com/500",
                    Detail = "Please contact support if the problem persists.",
                    Instance = context.Request.Path,
                    ErrorCode = "INTERNAL_SERVER_ERROR"
                };
                logLevel = LogLevel.Error;
                break;
        }

        problem.Extensions["traceId"] = context.TraceIdentifier;

        logger.Log(
            logLevel,
            ex,
            "Exception handled. Status: {Status}, ErrorCode: {ErrorCode}, Path: {Path}",
            problem.Status,
            problem.ErrorCode,
            context.Request.Path
        );

        context.Response.StatusCode = problem.Status!.Value;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problem);
    }
}
