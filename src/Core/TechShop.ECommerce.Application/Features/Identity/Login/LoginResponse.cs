using System.Text.Json.Serialization;

namespace TechShop.ECommerce.Application.Features.Identity.Login;

public sealed record LoginResponse(
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
