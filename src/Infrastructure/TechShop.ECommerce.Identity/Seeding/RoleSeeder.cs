using TechShop.ECommerce.Application.Common.Constants;

namespace TechShop.ECommerce.Identity.Seeding;

public sealed class RoleSeeder(RoleManager<IdentityRole<Guid>> roleManager) : IIdentitySeeder
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        string[] roles = { Roles.Admin, Roles.Customer };

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpperInvariant()
                });
            }
        }
    }
}