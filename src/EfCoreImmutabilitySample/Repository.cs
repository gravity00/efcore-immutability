using Microsoft.EntityFrameworkCore;

namespace EfCoreImmutabilitySample;

public class Repository<TEntity> where TEntity : Entity
{
    private readonly SampleDbContext _context;
    private readonly DbSet<TEntity> _set;

    public Repository(SampleDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        _set = _context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(long id, CancellationToken ct)
    {
        return await _set.FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken ct)
    {
        var entry = await _set.AddAsync(entity, ct);

        await _context.SaveChangesAsync(ct);

        entry.State = EntityState.Detached;
        return entry.Entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken ct)
    {
        var entry = _set.Update(entity);

        await _context.SaveChangesAsync(ct);

        entry.State = EntityState.Detached;
        return entry.Entity;
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken ct)
    {
        var entry = _set.Remove(entity);

        await _context.SaveChangesAsync(ct);

        entry.State = EntityState.Detached;
    }
}