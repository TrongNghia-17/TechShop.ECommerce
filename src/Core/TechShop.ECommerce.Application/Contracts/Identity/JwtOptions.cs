namespace TechShop.ECommerce.Application.Contracts.Identity;

public class JwtOptions
{
    public const string SectionName = "JwtSettings";

    [Required]
    public string Key { get; init; } = default!;

    [Required]
    public string Issuer { get; init; } = default!;

    [Required]
    public string Audience { get; init; } = default!;

    [Range(1, 1440)]
    public int DurationInMinutes { get; init; } = 15;

    [Range(1, 30)]
    public int RefreshTokenLifetimeInDays { get; init; } = 7;

    [Required]
    public string RefreshTokenCookieName { get; init; } = "refreshToken";
}
