namespace TechShop.ECommerce.Application.Features.Identity.Commands.Login;

public sealed class LoginCommandHandler(
    IIdentityService identityService,
    IJwtTokenGenerator tokenGenerator)
    : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var (Success, UserId, Email, UserName, Roles) = await identityService
            .LoginAsync(request.Email, request.Password);

        if (!Success)
            throw new BadRequestException("Invalid credentials.");

        var token = await tokenGenerator.GenerateTokenAsync(
            new UserTokenRequest(
                UserId,
                Email,
                Roles));

        return new LoginResponse(
            UserId,
            Email,
            UserName,
            token
        );
    }
}