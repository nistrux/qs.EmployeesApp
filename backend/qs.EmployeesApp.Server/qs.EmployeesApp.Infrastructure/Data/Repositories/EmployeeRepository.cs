using qs.EmployeesApp.Application.Contracts.Repositories;
using qs.EmployeesApp.Domain.Entities;

namespace qs.EmployeesApp.Infrastructure.Data.Repositories;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(EmpDbContext context) : base(context)
    {
    }
}