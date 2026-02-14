namespace TechShop.ECommerce.Identity.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
{

    /// <summary>
    /// Fixed IDs for deterministic EF Core migrations (HasData).
    /// </summary>
    public static class IdentitySeed
    {
        // Roles
        public static readonly Guid AdminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        public static readonly Guid UserRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        // Users
        public static readonly Guid AdminUserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        public static readonly Guid NormalUserId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

        public const string AdminRoleName = "Admin";
        public const string UserRoleName = "User";

        public const string AdminEmail = "admin@localhost.com";
        public const string UserEmail = "user@localhost.com";

        public const string DefaultPassword = "P@ssword1";
    }

    public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
    {
        builder.HasData(
               new IdentityRole<Guid>
               {
                   Id = IdentitySeed.AdminRoleId,
                   Name = IdentitySeed.AdminRoleName,
                   NormalizedName = IdentitySeed.AdminRoleName.ToUpperInvariant(),
                   ConcurrencyStamp = "role-admin-stamp"
               },
               new IdentityRole<Guid>
               {
                   Id = IdentitySeed.UserRoleId,
                   Name = IdentitySeed.UserRoleName,
                   NormalizedName = IdentitySeed.UserRoleName.ToUpperInvariant(),
                   ConcurrencyStamp = "role-user-stamp"
               }
           );
    }
}
