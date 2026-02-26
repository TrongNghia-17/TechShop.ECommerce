namespace TechShop.ECommerce.Api.Endpoints;

public static class CartEndpoints
{
    public static RouteGroupBuilder MapCartEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/carts")
            .WithTags("Carts")
            .RequireAuthorization();

        // POST /api/carts/items
        group.MapPost("/items",
            async ([FromBody] AddToCartCommand command, ISender sender, CancellationToken token) =>
            {
                var result = await sender.Send(command, token);
                return result.ToApiResult();
            })
            .WithName("Cart_AddItem")
            .WithSummary("Adds an item to the cart")
            .Produces<AddToCartResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);

        // DELETE /api/carts/items
        group.MapDelete("/items",
            async ([FromBody] RemoveFromCartCommand command, ISender sender, CancellationToken token) =>
            {
                var result = await sender.Send(command, token);
                return result.ToApiResult();
            })
            .WithName("Cart_RemoveItem")
            .WithSummary("Removes an item from the cart")
            .Produces<AddToCartResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);

        // GET /api/carts
        group.MapGet("/",
            async (ISender sender, CancellationToken token) =>
            {
                var result = await sender.Send(
                    new GetCartQuery(),
                    token);

                return result.ToApiResult();
            })
            .WithName("Cart_Get")
            .WithSummary("Gets current user's cart")
            .Produces<GetCartResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        return group;
    }
}