using Core.Entities;
using Core.Providers.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Core.Providers.Data.Interceptors;
internal class EntityTimestampableInterceptor : SaveChangesInterceptor
{
    private readonly TimeProvider _dateTime;

    public EntityTimestampableInterceptor(TimeProvider dateTime)
    {
        _dateTime = dateTime;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<IEntityTimestampable>())
        {
            if (entry.State is EntityState.Added or EntityState.Modified || entry.HasModifiedOwnedEntities())
            {
                var now = _dateTime.GetLocalNow();
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                }
                entry.Entity.LastModifiedAt = now;
            }
        }
    }
}
