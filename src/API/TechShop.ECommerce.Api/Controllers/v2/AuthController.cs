namespace TechShop.ECommerce.Api.Controllers.v2;

[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuthController(
    IAuthService authenticationService)
    : ControllerBase
{
    [HttpPost("login")]
    [EnableRateLimiting("AuthFixed")]
    public async Task<ActionResult<AuthResponse>> Login(AuthRequest request)
    {
        return Ok(await authenticationService.Login(request));
    }

    [HttpPost("register")]
    [EnableRateLimiting("AuthFixed")]
    public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
    {
        return Ok(await authenticationService.Register(request));
    }
}

