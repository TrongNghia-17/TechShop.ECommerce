namespace TechShop.ECommerce.Application.Contracts.Persistence;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    void SetConcurrencyToken<TEntity, TToken>(
        TEntity entity,
        Expression<Func<TEntity, TToken>> property,
        TToken originaValue)
        where TEntity : class;
    Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken token);
}
