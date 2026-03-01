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
                return result.ToApiResult();
            })
        .WithName("Products_GetPaged")
        .WithSummary("Gets paginated list of products")
        .Produces<PagedResponse<ProductDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .CacheOutput("ProductsList")
        .RequireRateLimiting("ProductsSliding");

        // GET /api/products/{id}
        group.MapGet("/{id:guid}",
            async (Guid id,
                ISender sender,
                CancellationToken token) =>
            {
                var result = await sender.Send(new GetProductDetailsQuery(id), token);
                return result.ToApiResult();
            })
        .WithName("Products_GetById")
        .WithSummary("Gets product details by id")
        .Produces<ProductDetailsDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .CacheOutput("ProductDetail");

        // PUT /api/products/{id}
        group.MapPut("/{id:guid}",
            async (
                Guid id,
                UpdateProductCommand command,
                ISender sender,
                IOutputCacheStore cacheStore,
                CancellationToken token) =>
            {
                var result = await sender.Send(command, token);
                await cacheStore.EvictByTagAsync("products", token);

                return result.ToApiResult();
            })
        .WithName("Products_Update")
        .WithSummary("Updates product")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest);

        return group;
    }
}