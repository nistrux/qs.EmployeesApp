using qs.EmployeesApp.Domain.Entities;
using System.Linq.Dynamic.Core;

namespace qs.EmployeesApp.Application.Contracts.Specifications;

public class EmployeesSortSpecification : SpecificationBase<Employee>
{
    public EmployeesSortSpecification(string? sortField, string? sortOrder)
        : base(query =>
        {
            if (string.IsNullOrEmpty(sortField))
            {
                return query.OrderBy(e => e.Id); // Default sort
            }

            if (sortField.Equals("salary") && !string.IsNullOrEmpty(sortOrder))
            {
                return sortOrder switch
                {
                    "asc" => query.OrderBy(x => x.Position!.BaseSalary + x.SalaryAdjustment),
                    "desc" => query.OrderByDescending(x => x.Position!.BaseSalary + x.SalaryAdjustment),
                    _ => query.OrderBy(x => x.Id)
                };
            }
            
            var orderBy = $"{sortField} {sortOrder}";
            try
            {
                return query.OrderBy(orderBy);
            }
            catch (Exception)
            {
                return query.OrderBy(e => e.Id); // Fallback to default
            }
        })
    {
    }
}