using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using TechShop.ECommerce.Application.Contracts.PaymentGateway;
using TechShop.ECommerce.Application.Features.Payments.StripeWebhook;

namespace TechShop.ECommerce.Api.Endpoints;

public static class StripeWebhookEndpoints
{
    public static RouteGroupBuilder MapStripeWebhookEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/webhooks")
            .WithTags("Webhooks");

        group.MapPost("/stripe", async (
            HttpRequest request,
            ISender sender,
            IOptions<StripeSettings> options,
            CancellationToken token) =>
        {
            var json = await new StreamReader(request.Body)
                .ReadToEndAsync(token);

            var signature = request.Headers["Stripe-Signature"];

            Event stripeEvent;

            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    signature,
                    options.Value.WebhookSecret);
            }
            catch (StripeException)
            {
                return Results.BadRequest();
            }

            if (stripeEvent.Type != EventTypes.CheckoutSessionCompleted)
                return Results.Ok();

            var session = stripeEvent.Data.Object as Session;
            if (session is null)
                return Results.BadRequest();

            if (!session.Metadata.TryGetValue("orderId", out var orderIdString))
            {
                return Results.BadRequest();
            }

            var orderId = Guid.Parse(orderIdString);

            var result = await sender.Send(
                new StripeWebhookCommand(
                    stripeEvent.Type,
                    session.Id,
                    orderId),
                token);

            return result.ToApiResult();
        })
        .WithName("Stripe_Webhook")
        .WithSummary("Stripe webhook endpoint")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

        return group;
    }
}