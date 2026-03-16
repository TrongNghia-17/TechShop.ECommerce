namespace TechShop.ECommerce.Identity.Authentication;

public class JwtTokenGenerator(IOptions<JwtOptions> jwtSettings)
    : IJwtTokenGenerator
{
    private readonly JwtOptions _jwtSettings = jwtSettings.Value;

    public Task<string> GenerateTokenAsync(UserTokenRequest request)
    {
        var claims = new List<Claim>
{
            new(JwtRegisteredClaimNames.Sub, request.UserId.ToString()),
            new("uid", request.UserId.ToString()),
            new(JwtRegisteredClaimNames.Email, request.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Add role claims
        claims.AddRange(
            request.Roles.Select(role =>
                new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Key));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return Task.FromResult(tokenString);
    }
}