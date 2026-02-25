namespace TechShop.ECommerce.Api.Endpoints;

public static class OrderEndpoints
{
    public static RouteGroupBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders")
            .WithTags("Orders")
            .RequireAuthorization();

        group.MapPost("/", CreateOrder)
            .WithName("Order_Create")
            .WithSummary("Create a new order");

        group.MapGet("/{id:guid}",
            () => TypedResults.StatusCode(StatusCodes.Status501NotImplemented))
            .WithName("Order_GetById");

        return group;
    }

    // ================================
    // Handlers
    // ================================

    private static async Task<Created<CreateOrderResponse>> CreateOrder(
        [FromBody] CreateOrderRequest request,
        [FromServices] IMediator mediator,
        [FromServices] IUserService userService,
        CancellationToken ct)
    {
        var command = new PlaceOrderCommand(
            CustomerId: userService.UserId,
            ShippingAddress: request.ShippingAddress,
            Notes: request.Notes
        );

        var orderId = await mediator.Send(command, ct);

        return TypedResults.Created(
            $"/api/orders/{orderId}",
            new CreateOrderResponse(orderId));
    }
}