using TechShop.ECommerce.Application.Common.Constants;

namespace TechShop.ECommerce.Identity.Services;

public class IdentityService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager)
    : IIdentityService
{
    public async Task<(bool Success, Guid UserId, string Email, string UserName, IList<string> Roles)>
        LoginAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
            return (false, Guid.Empty, "", "", new List<string>());

        var result = await signInManager
            .CheckPasswordSignInAsync(user, password, false);

        if (!result.Succeeded)
            return (false, Guid.Empty, "", "", new List<string>());

        var roles = await userManager.GetRolesAsync(user);

        return (true, user.Id, user.Email!, user.UserName!, roles);
    }

    public async Task<(bool Success, Guid UserId, string Errors)>
        RegisterAsync(
            string email,
            string userName,
            string firstName,
            string lastName,
            string password)
    {
        var user = new ApplicationUser
        {
            Email = email,
            UserName = userName,
            FirstName = firstName,
            LastName = lastName,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = string.Join("\n",
                result.Errors.Select(e => e.Description));

            return (false, Guid.Empty, errors);
        }

        await userManager.AddToRoleAsync(user, Roles.Customer);

        return (true, user.Id, string.Empty);
    }

}
