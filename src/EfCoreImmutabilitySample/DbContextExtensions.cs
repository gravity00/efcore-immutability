using Microsoft.EntityFrameworkCore;

namespace EfCoreImmutabilitySample;

public static class DbContextExtensions
{
    public static async Task<TEntity> CreateAsync<TEntity>(
        this DbContext context,
        TEntity entity,
        CancellationToken ct
    ) where TEntity : Entity => await context.SaveEntityStateAsync(
        entity,
        EntityState.Added,
        ct
    );

    public static async Task<TEntity> UpdateAsync<TEntity>(
        this DbContext context,
        TEntity entity,
        CancellationToken ct
    ) where TEntity : Entity => await context.SaveEntityStateAsync(
        entity,
        EntityState.Modified,
        ct
    );

    public static async Task DeleteAsync<TEntity>(
        this DbContext context,
        TEntity entity,
        CancellationToken ct
    ) where TEntity : Entity => await context.SaveEntityStateAsync(
        entity,
        EntityState.Deleted,
        ct
    );

    private static async Task<TEntity> SaveEntityStateAsync<TEntity>(
        this DbContext context,
        TEntity entity,
        EntityState state,
        CancellationToken ct
    ) where TEntity : Entity
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        var entry = context.Entry(entity);
        entry.State = state;

        await context.SaveChangesAsync(ct);

        entry.State = EntityState.Detached;

        return entry.Entity;
    }
}