namespace TechShop.ECommerce.Application.Contracts.Identity;

public interface ICurrentUserService
{
    Guid UserId { get; }
    string Email { get; }
}
