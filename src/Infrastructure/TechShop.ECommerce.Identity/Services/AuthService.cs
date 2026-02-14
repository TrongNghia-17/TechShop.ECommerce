namespace TechShop.ECommerce.Identity.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    IOptions<JwtSettings> jwtSettings,
    SignInManager<ApplicationUser> signInManager,
    IUnitOfWork unitOfWork)
    : IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task<AuthResponse> Login(AuthRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email)
            ?? throw new NotFoundException($"User with {request.Email} not found.", request.Email);

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (result.Succeeded == false)
        {
            throw new BadRequestException($"Credentials for '{request.Email} aren't valid'.");
        }

        JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

        var response = new AuthResponse
        {
            Id = user.Id,
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            Email = user.Email ?? string.Empty,
            UserName = user.UserName ?? string.Empty
        };

        return response;
    }

    public async Task<RegistrationResponse> Register(RegistrationRequest request)
    {
        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            EmailConfirmed = true
        };

        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var createResult = await userManager.CreateAsync(user, request.Password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join("\n", createResult.Errors.Select(e => e.Description));
                throw new BadRequestException(errors);
            }

            var roleResult = await userManager.AddToRoleAsync(user, "User");

            if (!roleResult.Succeeded)
            {
                var errors = string.Join("\n", roleResult.Errors.Select(e => e.Description));
                throw new BadRequestException(errors);
            }
        }, CancellationToken.None);

        return new RegistrationResponse() { UserId = user.Id };
    }

    private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
    {
        var userClaims = await userManager.GetClaimsAsync(user);
        var roles = await userManager.GetRolesAsync(user);

        var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();

        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString() ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
        }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
           issuer: _jwtSettings.Issuer,
           audience: _jwtSettings.Audience,
           claims: claims,
           expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
           signingCredentials: signingCredentials);
        return jwtSecurityToken;
    }

}
