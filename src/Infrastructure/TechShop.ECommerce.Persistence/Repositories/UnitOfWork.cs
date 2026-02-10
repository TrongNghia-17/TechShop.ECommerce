namespace TechShop.ECommerce.Persistence.Repositories;

public class UnitOfWork(TechShopDatabaseContext context) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException(
                "A concurrency conflict occurred.", ex);
        }
    }

    public void SetConcurrencyToken<TEntity, TToken>(
        TEntity entity,
        Expression<Func<TEntity,
            TToken>> property,
        TToken originaValue)
        where TEntity : class
    {
        context.Entry(entity)
            .Property(property)
            .OriginalValue = originaValue;
    }
}

