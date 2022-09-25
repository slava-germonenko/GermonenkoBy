using GermonenkoBy.Common.EntityFramework.Models;

using Microsoft.EntityFrameworkCore;

namespace GermonenkoBy.Common.EntityFramework;

public class BaseContext : DbContext
{
    public BaseContext(DbContextOptions options) : base(options) { }

    public override int SaveChanges()
    {
        SetUpdatedDate();
        SetCreatedDate();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        SetUpdatedDate();
        SetCreatedDate();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        SetUpdatedDate();
        SetCreatedDate();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new()
    )
    {
        SetUpdatedDate();
        SetCreatedDate();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void SetUpdatedDate()
    {
        var entities = ChangeTracker.Entries<IChangeDateTrackingModel>().Where(e => e.State == EntityState.Modified);
        foreach (var entity in entities)
        {
            entity.Entity.UpdatedDate = DateTime.UtcNow;
            entity.Property(prop => prop.CreatedDate).IsModified = false;
        }
    }

    private void SetCreatedDate()
    {
        var entities = ChangeTracker.Entries<IChangeDateTrackingModel>()
            .Where(e => e.State == EntityState.Added);

        foreach (var entity in entities)
        {
            entity.Entity.UpdatedDate = null;
            entity.Entity.CreatedDate = DateTime.UtcNow;
        }
    }
}