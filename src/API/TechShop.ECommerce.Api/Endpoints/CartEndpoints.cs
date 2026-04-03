using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using TechShop.ECommerce.Api.Extensions.Http;
using TechShop.ECommerce.Api.Extensions.RateLimiting;
using TechShop.ECommerce.Application.Features.Carts.AddToCart;
using TechShop.ECommerce.Application.Features.Carts.GetCart;
using TechShop.ECommerce.Application.Features.Carts.RemoveFromCart;
using TechShop.ECommerce.Application.Features.Carts.Shared;

namespace TechShop.ECommerce.Api.Endpoints;

public static class CartEndpoints
{
    public static RouteGroupBuilder MapCartEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/carts")
            .WithTags("Carts")
            .RequireAuthorization();

        group.MapPost("/items",
            async ([FromBody] AddToCartCommand command, ISender sender, CancellationToken token) =>
            {
                var result = await sender.Send(command, token);
                return result.ToApiResult();
            })
            .WithName("Cart_AddItem")
            .WithSummary("Adds an item to the cart")
            .Produces<CartSummaryResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .RequireRateLimiting(RateLimitPolicies.CartFixed);

        group.MapDelete("/items",
            async ([FromBody] RemoveFromCartCommand command, ISender sender, CancellationToken token) =>
            {
                var result = await sender.Send(command, token);
                return result.ToApiResult();
            })
            .WithName("Cart_RemoveItem")
            .WithSummary("Removes an item from the cart")
            .Produces<CartSummaryResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .RequireRateLimiting(RateLimitPolicies.CartFixed);

        group.MapGet("",
            async (ISender sender, CancellationToken token) =>
            {
                var result = await sender.Send(
                    new GetCartQuery(),
                    token);

                return result.ToApiResult();
            })
            .WithName("Cart_Get")
            .WithSummary("Gets current user's cart")
            .Produces<GetCartResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .RequireRateLimiting(RateLimitPolicies.CartFixed);

        return group;
    }
}