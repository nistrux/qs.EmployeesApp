namespace qs.EmployeesApp.Application.Contracts.Specifications;

public abstract class SpecificationBase<TResult> : ISpecification<TResult> where TResult : class
{
    private readonly Func<IQueryable<TResult>, IQueryable<TResult>> _query;

    protected SpecificationBase(Func<IQueryable<TResult>, IQueryable<TResult>> query)
    {
        _query = query ?? throw new ArgumentNullException(nameof(query));
    }

    public IQueryable<TResult> ApplyTo(IQueryable<TResult> set)
    {
        if (set is null)
        {
            throw new ArgumentNullException(nameof(set));
        }

        return _query(set);
    }
}