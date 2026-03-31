using System.ComponentModel.DataAnnotations;

namespace TechShop.ECommerce.Application.Common.Configurations.PaymentGateway;

public sealed class StripeOptions
{
    public const string SectionName = "StripeSettings";

    [Required]
    public string SecretKey { get; init; } = default!;

    [Required]
    public string WebhookSecret { get; init; } = default!;

    [Required]
    public string SuccessUrl { get; init; } = default!;

    [Required]
    public string CancelUrl { get; init; } = default!;

    [Required]
    public string Currency { get; init; } = "vnd";
}