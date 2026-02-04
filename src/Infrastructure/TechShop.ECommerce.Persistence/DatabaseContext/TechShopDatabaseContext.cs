using TechShop.ECommerce.Application.Contracts.Identity;

namespace TechShop.ECommerce.Persistence.DatabaseContext;

public class TechShopDatabaseContext(
    DbContextOptions<TechShopDatabaseContext> options,
     IUserService userService)
    : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TechShopDatabaseContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var userId = userService.UserId;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.MarkAsCreated(userId);
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property(x => x.DateCreated).IsModified = false;
                entry.Entity.MarkAsUpdated(userId);
            }
        }

        foreach (var entry in base.ChangeTracker.Entries<BaseEntity>()
            .Where(q => q.State == EntityState.Deleted))
        {
            entry.State = EntityState.Modified;
            entry.Entity.MarkAsDeleted(userId);
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
