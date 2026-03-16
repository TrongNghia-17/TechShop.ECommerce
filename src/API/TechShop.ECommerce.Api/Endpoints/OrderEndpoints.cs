using TechShop.ECommerce.Application.Features.Orders.Invoices;
using TechShop.ECommerce.Application.Features.Orders.PlaceOrder;

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
            .WithName("Orders_Create")
            .WithSummary("Creates a new order")
            .WithDescription("Places a new order for the current authenticated user.")
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status429TooManyRequests)
            .RequireRateLimiting(RateLimitPolicies.OrdersFixed);

        // GET /api/orders/{id}/invoice
        group.MapGet("/{id:guid}/invoice",
            async (
                Guid id,
                ISender sender,
                CancellationToken token) =>
            {
                var result = await sender.Send(new GetOrderInvoicePdfQuery(id), token);

                return Results.File(
                    fileContents: result.Content,
                    contentType: result.ContentType,
                    fileDownloadName: result.FileName);
            })
            .WithName("Orders_GetInvoicePdf")
            .WithSummary("Downloads invoice PDF for an order")
            .WithDescription("""
                Generates and downloads a PDF invoice for the specified order.

                Only confirmed orders can be exported as invoice PDFs.
                """)
            .Produces(StatusCodes.Status200OK, contentType: "application/pdf")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status429TooManyRequests)
            .RequireRateLimiting(RateLimitPolicies.OrdersFixed);

        return group;
    }
}