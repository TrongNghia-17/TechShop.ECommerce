using TechShop.ECommerce.Identity.Entities;

namespace TechShop.ECommerce.Identity.Seedings;

public sealed class UserSeeder(UserManager<ApplicationUser> userManager) : IIdentitySeeder
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        const string adminEmail = "admin@localhost.com";
        const string adminPassword = "P@ssword1";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser is not null)
        {
            return;
        }

        var user = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, adminPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException(errors);
        }

        await userManager.AddToRoleAsync(user, Roles.Admin);
    }
}