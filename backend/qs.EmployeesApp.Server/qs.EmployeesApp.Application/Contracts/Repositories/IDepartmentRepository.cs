using qs.EmployeesApp.Domain.Entities;

namespace qs.EmployeesApp.Application.Contracts.Repositories;

public interface IDepartmentRepository: IRepository<Department>,
    ISpecificationRepository<Department>;