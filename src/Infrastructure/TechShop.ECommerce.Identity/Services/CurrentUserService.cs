namespace TechShop.ECommerce.Identity.Services;

public class CurrentUserService(
    IHttpContextAccessor contextAccessor)
    : ICurrentUserService
{
    public Guid UserId
    {
        get
        {
            var id = contextAccessor.HttpContext?.User?
                .FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(id, out var guid)
                ? guid
                : Guid.Empty;
        }
    }

    public string Email
    {
        get
        {
            var user = contextAccessor.HttpContext?.User;

            return user?.FindFirstValue(ClaimTypes.Email)
                ?? user?.FindFirstValue("email")
                ?? string.Empty;
        }
    }
}