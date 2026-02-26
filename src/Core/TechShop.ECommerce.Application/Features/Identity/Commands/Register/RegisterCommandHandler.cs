namespace TechShop.ECommerce.Application.Features.Identity.Commands.Register;

public sealed class RegisterCommandHandler(
    IIdentityService identityService)
    : IRequestHandler<RegisterCommand, RegisterResponse>
{
    public async Task<RegisterResponse> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var (Success, UserId, Errors) = await identityService.RegisterAsync(
            request.Email,
            request.UserName,
            request.FirstName,
            request.LastName,
            request.Password);

        if (!Success)
            throw new BadRequestException(Errors);

        return new RegisterResponse(UserId);
    }
}