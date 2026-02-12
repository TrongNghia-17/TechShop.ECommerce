namespace TechShop.ECommerce.Persistence.DatabaseContext;

public class TechShopDatabaseContext(
    DbContextOptions<TechShopDatabaseContext> options)
    : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TechShopDatabaseContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
