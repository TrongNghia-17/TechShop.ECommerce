namespace TechShop.ECommerce.Api.Middleware;

public class ExceptionMiddleware(RequestDelegate next)
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

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        CustomProblemDetails problem = new();

        switch (ex)
        {
            case BadRequestException badRequestException:
                statusCode = HttpStatusCode.BadRequest;
                problem = new CustomProblemDetails
                {
                    Type = nameof(BadRequestException),
                    Title = badRequestException.Message,
                    Status = (int)statusCode,
                    Detail = badRequestException.InnerException?.Message,
                    Instance = httpContext.Request.Path,
                    Errors = badRequestException.ValidationErrors
                };
                break;
            case NotFoundException NotFound:
                statusCode = HttpStatusCode.NotFound;
                problem = new CustomProblemDetails
                {
                    Type = nameof(NotFoundException),
                    Title = NotFound.Message,
                    Status = (int)statusCode,
                    Detail = NotFound.InnerException?.Message,
                    Instance = httpContext.Request.Path
                };
                break;
            default:
                problem = new CustomProblemDetails
                {
                    Type = nameof(HttpStatusCode.InternalServerError),
                    Title = ex.Message,
                    Status = (int)statusCode,
                    Detail = ex.StackTrace,
                    Instance = httpContext.Request.Path
                };
                break;
        }

        httpContext.Response.StatusCode = (int)statusCode;
        await httpContext.Response.WriteAsJsonAsync(problem);
    }
}
