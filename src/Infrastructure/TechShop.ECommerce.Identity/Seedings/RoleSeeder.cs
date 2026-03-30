namespace TechShop.ECommerce.Identity.Seedings;

public sealed class RoleSeeder(RoleManager<IdentityRole<Guid>> roleManager) : IIdentitySeeder
{
    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        string[] roleNames = [Roles.Admin, Roles.Customer];

        foreach (var roleName in roleNames)
        {
            var exists = await roleManager.RoleExistsAsync(roleName);

            if (exists)
            {
                continue;
            }

            var role = new IdentityRole<Guid>
            {
                Name = roleName,
                NormalizedName = roleName.ToUpperInvariant()
            };

            await roleManager.CreateAsync(role);
        }
    }
}