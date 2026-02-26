namespace TechShop.ECommerce.Api.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth")
            .WithTags("Auth");

        group.MapPost("/login", Login)
            .WithName("Auth_Login")
            .WithSummary("User login")
            .Produces<LoginResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireRateLimiting("AuthFixed");

        group.MapPost("/register", Register)
            .WithName("Auth_Register")
            .WithSummary("User registration")
            .Produces<RegisterResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireRateLimiting("AuthFixed");

        return group;
    }

    // ==========================
    // Handlers
    // ==========================

    private static async Task<Ok<LoginResponse>> Login(
        [FromBody] LoginCommand command,
        [FromServices] ISender sender,
        CancellationToken token)
    {
        var result = await sender.Send(command, token);
        return TypedResults.Ok(result);
    }

    private static async Task<Ok<RegisterResponse>> Register(
        [FromBody] RegisterCommand command,
        [FromServices] ISender sender,
        CancellationToken token)
    {
        var result = await sender.Send(command, token);
        return TypedResults.Ok(result);
    }
}