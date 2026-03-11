using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using TechShop.ECommerce.Application.Contracts.Jobs;
using TechShop.ECommerce.Application.Contracts.PaymentGateway;

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
            IOptions<StripeSettings> options,
            IStripeWebhookJobs stripeWebhookJobs,
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
                return Results.BadRequest("Invalid Stripe webhook signature.");
            }

            if (stripeEvent.Type != EventTypes.CheckoutSessionCompleted)
                return Results.Ok();

            if (stripeEvent.Data.Object is not Session session)
                return Results.BadRequest("Invalid Stripe checkout session.");

            if (!session.Metadata.TryGetValue("orderId", out var orderIdRaw)
                || !Guid.TryParse(orderIdRaw, out var orderId))
            {
                return Results.BadRequest("orderId metadata is missing or invalid.");
            }

            await stripeWebhookJobs.EnqueueCheckoutSessionCompletedProcessing(
                session.Id,
                orderId,
                token);

            return Results.Ok();
        })
        .WithName("Stripe_Webhook")
        .WithSummary("Stripe webhook endpoint")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

        return group;
    }
}