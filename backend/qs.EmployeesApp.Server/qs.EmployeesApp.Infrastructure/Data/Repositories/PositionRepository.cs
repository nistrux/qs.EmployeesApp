using qs.EmployeesApp.Application.Contracts.Repositories;
using qs.EmployeesApp.Domain.Entities;

namespace qs.EmployeesApp.Infrastructure.Data.Repositories;

public class PositionRepository : RepositoryBase<Position>, IPositionRepository
{
    public PositionRepository(EmpDbContext context) : base(context)
    {
    }
}