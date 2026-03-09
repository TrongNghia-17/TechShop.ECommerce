namespace TechShop.ECommerce.Application.Features.Identity.Commands.Register;

public sealed class RegisterCommandHandler(
    IIdentityService identityService)
    : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    public async Task<Result<RegisterResponse>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var (Success, UserId, Errors) = await identityService.RegisterAsync(
            command.Email,
            command.UserName,
            command.FirstName,
            command.LastName,
            command.Password);

        if (!Success)
        {
            var message = string.Join(", ", Errors);
            return IdentityErrors.RegisterFailed(message);
        }

        return new RegisterResponse(UserId);
    }
}