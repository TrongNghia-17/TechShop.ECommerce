using System.ComponentModel.DataAnnotations;

namespace TechShop.ECommerce.Application.Common.Configurations.AI;

public sealed class AssistantOptions
{
    public const string SectionName = "AI:Assistant";

    [Required(ErrorMessage = "System Prompt for AI is mandatory.")]
    public string SystemPrompt { get; set; } = "Bạn là trợ lý tư vấn sản phẩm của TechShop, một cửa hàng điện tử. Chỉ trả lời dựa trên danh sách sản phẩm được cung cấp bên dưới.";

    [Required(ErrorMessage = "The default greeting is mandatory.")]
    public string DefaultGreeting { get; set; } = "Xin chào! Tôi có thể giúp gì cho bạn hôm nay? 😊";
}