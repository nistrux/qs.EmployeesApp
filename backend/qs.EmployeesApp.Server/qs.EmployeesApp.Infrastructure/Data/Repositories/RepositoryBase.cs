using qs.EmployeesApp.Application.Contracts.Repositories;

namespace qs.EmployeesApp.Infrastructure.Data.Repositories;

public abstract class RepositoryBase<TEntity> : ReadOnlyRepositoryBase<TEntity>, IRepository<TEntity> where TEntity : class
{
    protected RepositoryBase(EmpDbContext context) : base(context)
    {
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Context.Set<TEntity>().Add(entity);

        await Context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public virtual async Task<IReadOnlyCollection<TEntity>> AddRangeAsync(IReadOnlyCollection<TEntity> entities, CancellationToken cancellationToken = default)
    {
        Context.Set<TEntity>().AddRange(entities);

        await Context.SaveChangesAsync(cancellationToken);

        return entities;
    }

    public virtual async Task DeleteAsync<TKey>(TKey id, CancellationToken cancellationToken = default) where TKey : struct
    {
        var entity = await GetByIdAsync(id, cancellationToken);

        if (entity is null) return;
        
        await DeleteAsync(entity, cancellationToken);
    }

    public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Context.Set<TEntity>().Remove(entity);

        await Context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteRangeAsync(IReadOnlyCollection<TEntity> entities, CancellationToken cancellationToken = default)
    {
        Context.Set<TEntity>().RemoveRange(entities);

        await Context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Context.Set<TEntity>().Update(entity);

        await Context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task UpdateRangeAsync(IReadOnlyCollection<TEntity> entities, CancellationToken cancellationToken = default)
    {
        Context.Set<TEntity>().UpdateRange(entities);

        await Context.SaveChangesAsync(cancellationToken);
    }
}
