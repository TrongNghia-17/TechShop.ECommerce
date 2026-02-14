namespace TechShop.ECommerce.Persistence.Interceptors;

public sealed class AuditSoftDeleteSaveChangesInterceptor(
    IUserService UserService)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ApplyRules(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplyRules(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplyRules(DbContext? context)
    {
        if (context is null) return;

        var userId = UserService.UserId;

        ApplyAuditFields(context.ChangeTracker, userId);
        ApplySoftDelete(context.ChangeTracker, userId);
    }

    private static void ApplyAuditFields(ChangeTracker changeTracker, Guid? userId)
    {
        foreach (var entry in changeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.MarkAsCreated(userId);
                continue;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property(x => x.DateCreated).IsModified = false;
                entry.Entity.MarkAsUpdated(userId);
            }
        }
    }

    private static void ApplySoftDelete(ChangeTracker changeTracker, Guid? userId)
    {
        foreach (var entry in changeTracker.Entries<ISoftDelete>())
        {
            if (entry.State != EntityState.Deleted) continue;

            entry.State = EntityState.Modified;
            entry.Entity.MarkAsDeleted(userId);
        }
    }
}
