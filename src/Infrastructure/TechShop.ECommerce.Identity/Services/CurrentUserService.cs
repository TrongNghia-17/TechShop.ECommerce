namespace TechShop.ECommerce.Identity.Services;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid UserId
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User;
            var id = user?.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier") 
                ?? user?.FindFirstValue("oid")
                ?? user?.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? user?.FindFirstValue("sub");

            return Guid.TryParse(id, out var guid)
                ? guid
                : Guid.Empty;
        }
    }

    public string Email
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User;

            return user?.FindFirstValue(ClaimTypes.Email)
                ?? user?.FindFirstValue(JwtRegisteredClaimNames.Email)
                ?? string.Empty;
        }
    }
}