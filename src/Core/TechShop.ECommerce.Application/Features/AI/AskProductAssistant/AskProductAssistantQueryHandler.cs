using Microsoft.Extensions.Options;
using System.Text;
using TechShop.ECommerce.Application.Common.Configurations.AI;
using TechShop.ECommerce.Application.Contracts.AI;
using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Application.Features.AI.Shared;
using TechShop.ECommerce.Application.Features.Products.Shared;

namespace TechShop.ECommerce.Application.Features.AI.AskProductAssistant;

public sealed class AskProductAssistantQueryHandler(
    IEmbeddingProvider embeddingProvider,
    IPGVectorRepository vectorRepository,
    IChatProvider chatProvider,
    IOptions<AssistantOptions> assistantOptions)
    : IRequestHandler<AskProductAssistantQuery, ChatResponse>
{
    private readonly AssistantOptions _settings = assistantOptions.Value;

    private static readonly HashSet<string> Greetings =
    [
        "hi", "hello", "hey", "chào", "xin chào", "alo"
    ];

    public async Task<ChatResponse> Handle(
        AskProductAssistantQuery query,
        CancellationToken cancellationToken)
    {
        var normalizedQuestion = query.Question.Trim().ToLowerInvariant();

        if (Greetings.Contains(normalizedQuestion))
        {
            return new ChatResponse(_settings.DefaultGreeting);
        }

        var queryVector = await embeddingProvider.EmbedAsync(query.Question, cancellationToken);

        var relevantProducts = await vectorRepository.HybridSearchAsync(
            query.Question, queryVector, query.TopK, cancellationToken);

        var prompt = BuildPrompt(query.Question, relevantProducts, query.ChatHistory, _settings);
        var answer = await chatProvider.ChatAsync(prompt, cancellationToken);

        var sources = relevantProducts
            .Select(product => new SourceResult("DB", product.Name))
            .ToList();

        return new ChatResponse(answer, relevantProducts, sources);
    }

    private static string BuildPrompt(
        string question,
        IReadOnlyList<ProductSearchModel> products,
        IReadOnlyList<(string Role, string Content)>? chatHistory,
        AssistantOptions settings)
    {
        var prompt = new StringBuilder();

        prompt.AppendLine(settings.SystemPrompt);
        prompt.AppendLine();

        if (chatHistory is { Count: > 0 })
        {
            prompt.AppendLine("Lịch sử hội thoại trước:");
            foreach (var (role, content) in chatHistory)
            {
                prompt.AppendLine($"{role}: {content}");
            }
            prompt.AppendLine();
        }

        if (products.Count > 0)
        {
            prompt.AppendLine("Sản phẩm hiện có:");
            foreach (var product in products)
            {
                prompt.AppendLine($"- {product.Name}: {product.Description} (Giá: {product.Price:N0} VNĐ)");
            }
            prompt.AppendLine();
        }

        prompt.AppendLine($"Câu hỏi hiện tại của khách hàng: {question}");

        return prompt.ToString();
    }
}
