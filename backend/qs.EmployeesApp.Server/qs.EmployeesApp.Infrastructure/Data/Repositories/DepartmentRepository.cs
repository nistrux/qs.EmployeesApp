using qs.EmployeesApp.Application.Contracts.Repositories;
using qs.EmployeesApp.Domain.Entities;

namespace qs.EmployeesApp.Infrastructure.Data.Repositories;

public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
{
    public DepartmentRepository(EmpDbContext context) : base(context)
    {
    }
}