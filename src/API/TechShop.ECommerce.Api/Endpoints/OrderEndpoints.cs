namespace TechShop.ECommerce.Api.Endpoints;

public static class OrderEndpoints
{
    public static RouteGroupBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders")
            .WithTags("Orders")
            .RequireAuthorization();

        // POST /api/orders
        group.MapPost("/",
            async Task<Results<
                Created<Guid>,
                BadRequest>> (
                [FromBody] PlaceOrderCommand command,
                ISender sender,
                CancellationToken token) =>
            {
                var orderId = await sender.Send(command, token);

                return TypedResults.Created($"/api/orders/{orderId}", orderId);
            })
            .WithName("Order_Create")
            .WithSummary("Create a new order");

        return group;
    }
}