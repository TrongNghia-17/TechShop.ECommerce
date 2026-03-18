using TechShop.ECommerce.Application.Contracts.Identity;

namespace TechShop.ECommerce.Persistence.Interceptors;

public sealed class SoftDeleteSaveChangesInterceptor(
    ICurrentUserService currentUserService)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ApplySoftDelete(eventData.Context, currentUserService.UserId);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplySoftDelete(eventData.Context, currentUserService.UserId);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void ApplySoftDelete(DbContext? context, Guid? userId)
    {
        if (context is null)
        {
            return;
        }

        foreach (var entry in context.ChangeTracker.Entries<ISoftDelete>())
        {
            if (entry.State != EntityState.Deleted)
            {
                continue;
            }

            entry.State = EntityState.Modified;
            entry.Entity.MarkAsDeleted(userId);
        }
    }
}