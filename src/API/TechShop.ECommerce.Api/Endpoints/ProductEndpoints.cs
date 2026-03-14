using TechShop.ECommerce.Application.Features.Products.UploadProductImage;

namespace TechShop.ECommerce.Api.Endpoints;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products")
            .WithTags("Products");

        // GET /api/products
        group.MapGet("/",
            async (
                [AsParameters] GetProductsQuery query,
                ISender sender,
                CancellationToken token) =>
            {
                var result = await sender.Send(query, token);
                return Result<PagedResponse<ProductDto>>.Success(result);
            })
        .WithName("Products_GetPaged")
        .WithSummary("Gets paginated list of products")
        .Produces<PagedResponse<ProductDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireRateLimiting("ProductsSliding");

        // GET /api/products/{id}
        group.MapGet("/{id:guid}",
            async (
                Guid id,
                ISender sender,
                CancellationToken token) =>
            {
                var result = await sender.Send(new GetProductDetailsQuery(id), token);
                return Result<ProductDetailsDto>.Success(result);
            })
        .WithName("Products_GetById")
        .WithSummary("Gets product details by id")
        .Produces<ProductDetailsDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // PUT /api/products/{id}
        group.MapPut("/{id:guid}",
            async (
                Guid id,
                UpdateProductCommand command,
                ISender sender,
                CancellationToken token) =>
            {
                var result = await sender.Send(command, token);
                return result.ToApiResult();
            })
        .WithName("Products_Update")
        .WithSummary("Updates product")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest);

        // POST /api/products/{id}/image
        group.MapPost("/{id:guid}/image",
            async (
                Guid id,
                IFormFile file,
                ISender sender,
                CancellationToken token) =>
            {
                if (file is null || file.Length == 0)
                    return Results.BadRequest("File is required.");

                await using var stream = file.OpenReadStream();

                var command = new UploadProductImageCommand(
                    id,
                    stream,
                    file.FileName,
                    file.ContentType,
                    file.Length);

                var result = await sender.Send(command, token);

                return result.ToApiResult();
            })
        .WithName("Products_UploadImage")
        .WithSummary("Uploads product main image")
        .WithDescription("""
            Uploads a main image for the specified product using multipart/form-data.

            The request must include a file field named 'file'.
            Supported validation rules are handled in the application layer.
            If the product already has a main image, the old blob will be replaced.
            """)
        .Accepts<IFormFile>("multipart/form-data")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .DisableAntiforgery();

        return group;
    }
}