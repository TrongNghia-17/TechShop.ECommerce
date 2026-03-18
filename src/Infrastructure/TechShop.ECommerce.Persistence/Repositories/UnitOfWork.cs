namespace TechShop.ECommerce.Persistence.Repositories;

public sealed class UnitOfWork(TechShopDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}

