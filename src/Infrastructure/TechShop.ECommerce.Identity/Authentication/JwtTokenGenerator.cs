namespace TechShop.ECommerce.Identity.Authentication;

public class JwtTokenGenerator(IOptions<JwtOptions> options)
    : IJwtTokenGenerator
{
    private readonly JwtOptions _jwtOptions = options.Value;

    public Task<string> GenerateTokenAsync(UserTokenRequest request)
    {
        var claims = BuildClaims(request);

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtOptions.Key));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.DurationInMinutes),
            signingCredentials: credentials);

        var tokenValue = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return Task.FromResult(tokenValue);
    }

    private static List<Claim> BuildClaims(UserTokenRequest request)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, request.UserId.ToString()),
            new(ClaimTypes.NameIdentifier, request.UserId.ToString()),
            new(JwtRegisteredClaimNames.Email, request.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange(
            request.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }
}