using qs.EmployeesApp.Domain.Entities;

namespace qs.EmployeesApp.Application.Contracts.Specifications;

public class EmployeesFilterSpecification : SpecificationBase<Employee>
{
    public EmployeesFilterSpecification(string? filterField, string? filterValue)
        : base(query =>
        {
            if (string.IsNullOrEmpty(filterValue))
            {
                return query;
            }

            return filterField?.ToLowerInvariant() switch
            {
                "name" => query.Where(e => e.Name.ToLower().Contains(filterValue.ToLower())),
                "position" => query.Where(e =>
                    e.Position != null && e.Position.Name.ToLower().Contains(filterValue.ToLower())),
                _ => query // Default: no filter
            };
        })
    {
    }
}