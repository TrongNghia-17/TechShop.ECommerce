namespace TechShop.ECommerce.Identity.DbContext;

public class TechShopIdentityDbContext(
    DbContextOptions<TechShopIdentityDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(TechShopIdentityDbContext).Assembly);
    }
}
