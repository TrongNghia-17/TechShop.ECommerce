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
}