using System.Text;
using TechShop.ECommerce.Application.Contracts.AI;
using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Application.Features.AI.Shared;
using TechShop.ECommerce.Application.Features.Products.Shared;

namespace TechShop.ECommerce.Application.Features.AI.AskProductAssistant;

public sealed class AskProductAssistantQueryHandler(
    IEmbeddingProvider embeddingProvider,
    IPGVectorRepository vectorRepository,
    IChatProvider chatProvider)
    : IRequestHandler<AskProductAssistantQuery, ChatResponse>
{
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
            return new ChatResponse("Xin chào! Tôi là trợ lý của TechShop. Tôi có thể giúp gì cho bạn hôm nay? 😊");
        }

        var queryVector = await embeddingProvider.EmbedAsync(query.Question, cancellationToken);

        var relevantProducts = await vectorRepository.HybridSearchAsync(
            query.Question, queryVector, query.TopK, cancellationToken);

        var prompt = BuildPrompt(query.Question, relevantProducts, query.ChatHistory);
        var answer = await chatProvider.ChatAsync(prompt, cancellationToken);

        var sources = relevantProducts
            .Select(product => new SourceResult("DB", product.Name))
            .ToList();

        return new ChatResponse(answer, relevantProducts, sources);
    }

    private static string BuildPrompt(
        string question,
        IReadOnlyList<ProductSearchModel> products,
        IReadOnlyList<(string Role, string Content)>? chatHistory)
    {
        var prompt = new StringBuilder();

        prompt.AppendLine("Bạn là trợ lý tư vấn sản phẩm của TechShop, một cửa hàng điện tử.");
        prompt.AppendLine("Chỉ trả lời dựa trên danh sách sản phẩm được cung cấp bên dưới.");
        prompt.AppendLine("Trả lời tự nhiên như một người tư vấn, không liệt kê sản phẩm theo dạng danh sách.");
        prompt.AppendLine("Nếu không có sản phẩm phù hợp, hãy lịch sự thông báo bạn không có thông tin đó.");
        prompt.AppendLine("Luôn trả lời bằng tiếng Việt.");
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

        prompt.AppendLine($"Câu hỏi của khách hàng: {question}");

        return prompt.ToString();
    }
}
