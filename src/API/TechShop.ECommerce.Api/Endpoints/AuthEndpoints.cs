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
            async ([FromBody] LoginCommand command,
                   ISender sender,
                   CancellationToken token) =>
            {
                var result = await sender.Send(command, token);
                return result.ToApiResult();
            })
            .WithName("Auth_Login")
            .WithSummary("User login")
            .Produces<LoginResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireRateLimiting("AuthFixed");

        // POST /api/auth/register
        group.MapPost("/register",
            async ([FromBody] RegisterCommand command,
                   ISender sender,
                   CancellationToken token) =>
            {
                var result = await sender.Send(command, token);
                return result.ToApiResult();
            })
            .WithName("Auth_Register")
            .WithSummary("User registration")
            .Produces<RegisterResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict)
            .RequireRateLimiting("AuthFixed");

        return group;
    }
}