namespace TechShop.ECommerce.Api.Endpoints;

public static class CartEndpoints
{
    public static RouteGroupBuilder MapCartEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/carts")
            .WithTags("Carts")
            .RequireAuthorization();

        // POST /api/carts/items
        group.MapPost("/items", AddItem)
            .WithName("Cart_AddItem")
            .WithSummary("Adds an item to the cart")
            .WithDescription("Adds a product with specified quantity to the current user's cart.")
            .Produces<AddToCartResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        // DELETE /api/carts/items
        group.MapDelete("/items", RemoveItem)
            .WithName("Cart_RemoveItem")
            .WithSummary("Removes an item from the cart")
            .WithDescription("Removes a product with specified quantity from the current user's cart.")
            .Produces<AddToCartResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        // GET /api/carts
        group.MapGet("/", GetCart)
            .WithName("Cart_Get")
            .WithSummary("Gets current user's cart")
            .WithDescription("Returns the current authenticated user's shopping cart.")
            .Produces<GetCartResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        return group;
    }

    // ============================
    // Handlers
    // ============================

    private static async Task<Ok<AddToCartResult>> AddItem(
        [FromBody] CartItemRequest request,
        [FromServices] IMediator mediator,
        [FromServices] IUserService userService,
        CancellationToken ct)
    {
        var command = new AddToCartCommand(
            CustomerId: userService.UserId,
            ProductId: request.ProductId,
            Quantity: request.Quantity);

        var result = await mediator.Send(command, ct);

        return TypedResults.Ok(result);
    }

    private static async Task<Ok<AddToCartResult>> RemoveItem(
        [FromBody] CartItemRequest request,
        [FromServices] IMediator mediator,
        [FromServices] IUserService userService,
        CancellationToken ct)
    {
        var command = new RemoveFromCartCommand(
            CustomerId: userService.UserId,
            ProductId: request.ProductId,
            Quantity: request.Quantity);

        var result = await mediator.Send(command, ct);

        return TypedResults.Ok(result);
    }

    private static async Task<Ok<GetCartResult>> GetCart(
        [FromServices] IMediator mediator,
        [FromServices] IUserService userService,
        CancellationToken ct)
    {
        var query = new GetCartQuery(userService.UserId);

        var result = await mediator.Send(query, ct);

        return TypedResults.Ok(result);
    }
}