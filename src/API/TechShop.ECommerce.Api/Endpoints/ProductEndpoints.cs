using TechShop.ECommerce.Application.Features.Products.Queries.GetProducts;

namespace TechShop.ECommerce.Api.Endpoints;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products")
            .WithTags("Products");

        // GET /api/products
        group.MapGet("/",
            async ([AsParameters] GetProductsQuery query,
                   ISender sender,
                   CancellationToken token) =>
            {
                var result = await sender.Send(query, token);
                return Results.Ok(result);
            })
            .WithName("Products_GetPaged")
            .WithSummary("Gets paginated list of products")
            .Produces<PagedResponse<ProductDto>>(StatusCodes.Status200OK)
            .CacheOutput("ProductsList")
            .RequireRateLimiting("ProductsSliding");

        // GET /api/products/{id}
        group.MapGet("/{id:guid}",
            async (Guid id,
                   ISender sender,
                   CancellationToken token) =>
            {
                var result = await sender.Send(new GetProductDetailsQuery(id), token);
                return Results.Ok(result);
            })
            .WithName("Products_GetById")
            .WithSummary("Gets product details by id")
            .Produces<ProductDetailsDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .CacheOutput("ProductDetail");

        return group;
    }
}