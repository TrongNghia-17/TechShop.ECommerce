namespace TechShop.ECommerce.Application.Features.Identity.Commands.Login;

public sealed class LoginCommandHandler(
    IIdentityService identityService,
    IJwtTokenGenerator tokenGenerator)
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var (Success, UserId, Email, UserName, Roles) =
            await identityService.LoginAsync(
                command.Email,
                command.Password);

        if (!Success)
            return DomainErrors.Identity.InvalidCredentials;

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