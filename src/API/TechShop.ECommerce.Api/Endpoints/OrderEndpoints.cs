public static class OrderEndpoints
{
    public static RouteGroupBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders")
            .WithTags("Orders")
            .RequireAuthorization();

        group.MapPost("/", CreateOrder)
            .WithName("Order_Create")
            .WithSummary("Create a new order")
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        return group;
    }

    private static async Task<Created<CreateOrderResponse>> CreateOrder(
        [FromBody] CreateOrderRequest request,
        [FromServices] IMediator mediator,
        [FromServices] ICurrentUserService currentUserService,
        CancellationToken token)
    {
        var command = new PlaceOrderCommand(
            CustomerId: currentUserService.UserId,
            ShippingAddress: request.ShippingAddress,
            Notes: request.Notes
        );

        var orderId = await mediator.Send(command, token);

        return TypedResults.Created(
            $"/api/orders/{orderId}",
            new CreateOrderResponse(orderId));
    }
}