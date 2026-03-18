using TechShop.ECommerce.Domain.Entities.Carts;
using TechShop.ECommerce.Domain.Entities.Catalogs;

namespace TechShop.ECommerce.Persistence.Context;

public sealed class TechShopDbContext(DbContextOptions<TechShopDbContext> options)
    : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TechShopDbContext).Assembly);
    }
}
