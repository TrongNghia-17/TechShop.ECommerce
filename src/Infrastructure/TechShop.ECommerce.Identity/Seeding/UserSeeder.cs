namespace TechShop.ECommerce.Identity.Seeding;

public sealed class UserSeeder(UserManager<ApplicationUser> userManager) : IIdentitySeeder
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var adminEmail = "admin@localhost.com";
        var adminPassword = "P@ssword1";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser is null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Admin"
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (!result.Succeeded)
                throw new Exception(string.Join(",", result.Errors.Select(e => e.Description)));

            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}