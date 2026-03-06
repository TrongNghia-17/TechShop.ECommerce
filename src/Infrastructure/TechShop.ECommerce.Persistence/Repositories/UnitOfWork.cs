using TechShop.ECommerce.Persistence.DatabaseContext;

namespace TechShop.ECommerce.Persistence.Repositories;

public sealed class UnitOfWork(TechShopDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken token = default)
    {
        try
        {
            return await context.SaveChangesAsync(token);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("A concurrency conflict occurred.", ex);
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

    public async Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken token)
    {
        var strategy = context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            using var transaction = await context.Database.BeginTransactionAsync(token);
            try
            {
                await action();
                await transaction.CommitAsync(token);
            }
            catch
            {
                await transaction.RollbackAsync(token);
                throw;
            }
        });
    }
}

