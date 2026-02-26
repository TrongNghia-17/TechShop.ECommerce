namespace TechShop.ECommerce.Api.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth")
            .WithTags("Auth");

        // POST /api/auth/login
        group.MapPost("/login",
            async Task<Results<
                Ok<LoginResponse>,
                BadRequest>> (
                [FromBody] LoginCommand command,
                ISender sender,
                CancellationToken token) =>
            {
                var result = await sender.Send(command, token);
                return TypedResults.Ok(result);
            })
            .WithName("Auth_Login")
            .WithSummary("User login")
            .RequireRateLimiting("AuthFixed");

        // POST /api/auth/register
        group.MapPost("/register",
            async Task<Results<
                Ok<RegisterResponse>,
                BadRequest>> (
                [FromBody] RegisterCommand command,
                ISender sender,
                CancellationToken token) =>
            {
                var result = await sender.Send(command, token);
                return TypedResults.Ok(result);
            })
            .WithName("Auth_Register")
            .WithSummary("User registration")
            .RequireRateLimiting("AuthFixed");

        return group;
    }
}