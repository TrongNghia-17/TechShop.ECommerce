namespace TechShop.ECommerce.Identity.DbContext;

public class TechShopIdentityDbContext(
    DbContextOptions<TechShopIdentityDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(TechShopIdentityDbContext).Assembly);
    }
}
