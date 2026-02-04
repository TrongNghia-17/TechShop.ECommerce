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

        modelBuilder.Entity<Product>()
            .HasQueryFilter("SoftDelete", p => !p.IsDeleted);

        modelBuilder.Entity<Category>()
            .HasQueryFilter("SoftDelete", c => !c.IsDeleted);

        modelBuilder.Entity<Order>()
            .HasQueryFilter("SoftDelete", o => !o.IsDeleted);

        modelBuilder.Entity<OrderItem>()
            .HasQueryFilter("SoftDelete", oi => !oi.IsDeleted);

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
