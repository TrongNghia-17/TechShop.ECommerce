namespace TechShop.ECommerce.Application.Contracts.PaymentGateway;

public sealed class StripeOptions
{
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