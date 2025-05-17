namespace qs.EmployeesApp.Application.Contracts.Specifications;

public class PagingSpecification<T> : SpecificationBase<T> where T : class
{
    public PagingSpecification(int pageNumber, int pageSize)
        : base(query => query.Skip((pageNumber - 1) * pageSize).Take(pageSize))
    {
    }
}