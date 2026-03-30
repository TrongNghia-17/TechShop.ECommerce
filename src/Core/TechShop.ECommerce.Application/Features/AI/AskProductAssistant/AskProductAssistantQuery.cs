using TechShop.ECommerce.Application.Features.AI.Shared;

namespace TechShop.ECommerce.Application.Features.AI.AskProductAssistant;

public sealed record AskProductAssistantQuery(
    string Question,
    int TopK = 3,
    IReadOnlyList<(string Role, string Content)>? ChatHistory = null)
    : IRequest<ChatResponse>;
