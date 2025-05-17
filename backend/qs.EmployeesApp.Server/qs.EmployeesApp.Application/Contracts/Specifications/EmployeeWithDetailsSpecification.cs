using qs.EmployeesApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace qs.EmployeesApp.Application.Contracts.Specifications;

public class EmployeeWithDetailsSpecification : SpecificationBase<Employee>
{
    public EmployeeWithDetailsSpecification(long employeeId)
        : base(query => query
            .Include(e => e.Position)
            .Include(e => e.Department)
            .Where(e => e.Id == employeeId)
        )
    {
    }
}