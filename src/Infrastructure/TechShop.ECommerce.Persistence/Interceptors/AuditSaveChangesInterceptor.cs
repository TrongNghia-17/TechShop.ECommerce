namespace TechShop.ECommerce.Persistence.Interceptors;

public sealed class AuditSaveChangesInterceptor(
    IUserService userService)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ApplyAudit(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplyAudit(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplyAudit(DbContext? context)
    {
        if (context is null) return;

        var userId = userService.UserId;

        foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
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
}