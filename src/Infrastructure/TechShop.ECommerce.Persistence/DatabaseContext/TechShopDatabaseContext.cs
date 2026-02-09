using TechShop.ECommerce.Domain.Abstractions;

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

        ApplySoftDeleteFilters(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private static void ApplySoftDeleteFilters(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;
            if (clrType is null) continue;
            if (!typeof(ISoftDelete).IsAssignableFrom(clrType)) continue;

            modelBuilder.Entity(clrType).HasQueryFilter(BuildNotDeletedLambda(clrType));
        }
    }

    private static LambdaExpression BuildNotDeletedLambda(Type entityClrType)
    {
        var param = Expression.Parameter(entityClrType, "e");
        var isDeleted = Expression.Property(param, nameof(ISoftDelete.IsDeleted));
        var notDeleted = Expression.Not(isDeleted);
        return Expression.Lambda(notDeleted, param);
    }
}
