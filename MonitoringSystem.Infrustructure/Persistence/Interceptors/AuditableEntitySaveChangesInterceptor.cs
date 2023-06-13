using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Domein.Common.BaseEntities;

namespace MonitoringSystem.Infrustructure.Persistence.Interceptors;

public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{

    private readonly IDateTime _dateTime;
    private readonly IGuidGenerator _guidGenerator;

    public AuditableEntitySaveChangesInterceptor(
        IDateTime dateTime,
        IGuidGenerator guidGenerator)
    {
        _dateTime = dateTime;
        _guidGenerator = guidGenerator;
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

    public void UpdateEntities(DbContext context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State is EntityState.Added)
            {
                entry.Entity.CreatedDate = _dateTime.Now;
                entry.Entity.Id = _guidGenerator.Guid;
            }

            if (entry.State is EntityState.Added || entry.State is EntityState.Modified)
            {
                entry.Entity.UpdatedDate = _dateTime.Now;
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}



