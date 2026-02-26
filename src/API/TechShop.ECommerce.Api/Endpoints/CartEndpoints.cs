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
            async Task<Results<
                Ok<AddToCartResult>,
                NotFound>> (
                [FromBody] AddToCartCommand command,
                ISender sender,
                CancellationToken token) =>
            {
                var result = await sender.Send(command, token);
                return TypedResults.Ok(result);
            })
            .WithName("Cart_AddItem")
            .WithSummary("Adds an item to the cart");

        // DELETE /api/carts/items
        group.MapDelete("/items",
            async Task<Results<
                Ok<AddToCartResult>,
                NotFound>> (
                [FromBody] RemoveFromCartCommand command,
                ISender sender,
                CancellationToken token) =>
            {
                var result = await sender.Send(command, token);
                return TypedResults.Ok(result);
            })
            .WithName("Cart_RemoveItem")
            .WithSummary("Removes an item from the cart");

        // GET /api/carts
        group.MapGet("/",
            async Task<Results<
                Ok<GetCartResult>,
                NotFound>> (
                ISender sender,
                CancellationToken token) =>
            {
                var result = await sender.Send(
                    new GetCartQuery(),
                    token);

                return TypedResults.Ok(result);
            })
            .WithName("Cart_Get")
            .WithSummary("Gets current user's cart");

        return group;
    }
}