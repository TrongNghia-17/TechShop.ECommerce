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
            async ([FromBody] PlaceOrderCommand command, ISender sender, CancellationToken token) =>
            {
                var result = await sender.Send(command, token);

                return result.ToCreatedResult(id => $"/api/orders/{id}");
            })
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        return group;
    }
}