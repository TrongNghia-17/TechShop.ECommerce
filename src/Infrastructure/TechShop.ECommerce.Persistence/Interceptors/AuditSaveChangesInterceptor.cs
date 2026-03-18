using TechShop.ECommerce.Application.Contracts.Identity;

namespace TechShop.ECommerce.Persistence.Interceptors;

public sealed class AuditSaveChangesInterceptor(
    ICurrentUserService currentUserService)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ApplyAudit(eventData.Context, currentUserService.UserId);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplyAudit(eventData.Context, currentUserService.UserId);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void ApplyAudit(DbContext? context, Guid? userId)
    {
        if (context is null)
        {
            return;
        }

        foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.MarkAsCreated(userId);
                continue;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property(entity => entity.DateCreated).IsModified = false;
                entry.Entity.MarkAsUpdated(userId);
            }
        }
    }
}