using Microsoft.EntityFrameworkCore;
using qs.EmployeesApp.Application.Contracts.Repositories;
using qs.EmployeesApp.Application.Contracts.Specifications;

namespace qs.EmployeesApp.Infrastructure.Data.Repositories;

public abstract class ReadOnlyRepositoryBase<TEntity> : IReadOnlyRepository<TEntity>, ISpecificationRepository<TEntity> where TEntity : class
{
    protected readonly EmpDbContext Context;

    protected ReadOnlyRepositoryBase(EmpDbContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public virtual async Task<TEntity?> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default) where TKey : struct
    {
        return await Context.Set<TEntity>().FindAsync([id], cancellationToken);
    }

    public virtual async Task<TEntity?> GetBySpecificationAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> ListAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    public virtual async Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        if (specification is null)
        {
            throw new ArgumentNullException(nameof(specification));
        }

        return await ApplySpecification(specification).CountAsync(cancellationToken: cancellationToken);
    }

    private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
    {
        if (specification is null)
        {
            throw new ArgumentNullException(nameof(specification));
        }

        return specification.ApplyTo(Context.Set<TEntity>().AsQueryable());
    }
}