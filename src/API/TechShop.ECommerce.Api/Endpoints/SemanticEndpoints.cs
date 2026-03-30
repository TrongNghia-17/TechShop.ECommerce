using TechShop.ECommerce.Application.Common.Results;
using TechShop.ECommerce.Application.Contracts.AI;
using TechShop.ECommerce.Application.Features.AI.AskProductAssistant;
using TechShop.ECommerce.Application.Features.AI.Shared;
using TechShop.ECommerce.Application.Features.Products.IngestProductVectors;
using TechShop.ECommerce.Application.Features.Products.SearchProductsHybrid;
using TechShop.ECommerce.Application.Features.Products.Shared;

namespace TechShop.ECommerce.Api.Endpoints;

public sealed record SearchRequest(string Query, int TopK = 5);
public sealed record ChatRequest(string Question, int TopK = 3);
public sealed record EmbeddingTestRequest(string Text);

public static class SemanticEndpoints
{
    public static RouteGroupBuilder MapSemanticEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/ai")
            .WithTags("AI");

        // POST /api/ai/ingest
        group.MapPost("/ingest",
            async (
                ISender sender,
                CancellationToken token) =>
            {
                var result = await sender.Send(new IngestProductVectorsCommand(), token);
                return Result<int>.Success(result.Value);
            })
        .WithName("AI_IngestProductVectors")
        .WithSummary("Ingests all products into the vector database for semantic search")
        .Produces<int>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        // POST /api/ai/search/hybrid
        group.MapPost("/search/hybrid",
            async (
                SearchRequest request,
                ISender sender,
                CancellationToken token) =>
            {
                var results = await sender.Send(new SearchProductsHybridQuery(request.Query, request.TopK), token);
                return Results.Ok(results);
            })
        .WithName("AI_SearchHybrid")
        .WithSummary("Hybrid search — tries keyword match first, falls back to vector search")
        .Produces<IReadOnlyList<ProductSearchModel>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        // POST /api/ai/chat
        group.MapPost("/chat",
            async (
                ChatRequest request,
                ISender sender,
                CancellationToken token) =>
            {
                var result = await sender.Send(new AskProductAssistantQuery(request.Question, request.TopK), token);
                return Results.Ok(result);
            })
        .WithName("AI_Chat")
        .WithSummary("Ask the TechShop AI assistant a question about products (RAG)")
        .Produces<ChatResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        // POST /api/ai/embeddings/test (Development only — sanity check)
        // Only register this if we are in development environment
        if (app is WebApplication webApp && webApp.Environment.IsDevelopment())
        {
            group.MapPost("/embeddings/test",
                async (
                    EmbeddingTestRequest request,
                    IEmbeddingProvider embeddingProvider,
                    CancellationToken token) =>
                {
                    var vector = await embeddingProvider.EmbedAsync(request.Text, token);

                    return Results.Ok(new
                    {
                        Provider = embeddingProvider.GetType().Name,
                        ConfiguredDimensions = embeddingProvider.Dimensions,
                        ActualLength = vector.Length,
                        Status = "Connected Successfully"
                    });
                })
            .WithName("AI_EmbeddingTest")
            .WithSummary("DEV TOOL: Verifies AI provider connectivity")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);
        }

        return group;
    }
}
