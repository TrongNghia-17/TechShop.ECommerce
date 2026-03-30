using TechShop.ECommerce.Application.Common.Results;
using TechShop.ECommerce.Domain.Errors;

namespace TechShop.ECommerce.Api.Extensions.Http;

public static class ResultExtensions
{
    public static IResult ToApiResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return result.Error switch
        {
            NotFoundError => Results.NotFound(CreateProblemDetails(result.Error, 404)),
            ValidationError => Results.BadRequest(CreateProblemDetails(result.Error, 400)),
            ConflictError => Results.Conflict(CreateProblemDetails(result.Error, 409)),
            UnauthorizedError => Results.Json(
                CreateProblemDetails(result.Error, 401),
                statusCode: 401),
            _ => Results.BadRequest(CreateProblemDetails(result.Error, 400))
        };
    }

    public static IResult ToApiResult(this Result result, object? successValue = null)
    {
        if (result.IsSuccess)
        {
            return successValue is not null
                ? Results.Ok(successValue)
                : Results.Ok();
        }

        return result.Error switch
        {
            NotFoundError => Results.NotFound(CreateProblemDetails(result.Error, 404)),
            ValidationError => Results.BadRequest(CreateProblemDetails(result.Error, 400)),
            ConflictError => Results.Conflict(CreateProblemDetails(result.Error, 409)),
            _ => Results.BadRequest(CreateProblemDetails(result.Error, 400))
        };
    }

    public static IResult ToCreatedResult<T>(
        this Result<T> result,
        Func<T, string> locationFactory)
    {
        if (result.IsSuccess)
        {
            return Results.Created(locationFactory(result.Value), result.Value);
        }

        return result.ToApiResult();
    }

    private static ProblemDetails CreateProblemDetails(DomainErrors error, int statusCode) =>
        new()
        {
            Status = statusCode,
            Title = error.Code,
            Detail = error.Description,
            Type = $"https://httpstatuses.com/{statusCode}"
        };
}
