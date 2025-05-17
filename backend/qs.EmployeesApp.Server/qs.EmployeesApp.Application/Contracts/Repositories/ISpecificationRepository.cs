using qs.EmployeesApp.Application.Contracts.Specifications;

namespace qs.EmployeesApp.Application.Contracts.Repositories;

public interface ISpecificationRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetBySpecificationAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> ListAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
}