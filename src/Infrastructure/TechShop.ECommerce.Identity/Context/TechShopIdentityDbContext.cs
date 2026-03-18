namespace TechShop.ECommerce.Identity.Context;

public sealed class TechShopIdentityDbContext(
    DbContextOptions<TechShopIdentityDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TechShopIdentityDbContext).Assembly);
    }
}
