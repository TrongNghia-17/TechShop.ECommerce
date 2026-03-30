using System.Text.Json.Serialization;

namespace TechShop.ECommerce.Application.Features.Identity.RefreshToken;

public sealed record RefreshTokenResponse(
    Guid UserId,
    string Email,
    string UserName,
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAtUtc,
    DateTimeOffset RefreshTokenExpiresAtUtc)
{
    [JsonIgnore]
    public string RefreshToken { get; init; } = string.Empty;
}
