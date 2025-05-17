using qs.EmployeesApp.Domain.Entities;

namespace qs.EmployeesApp.Application.Contracts.Repositories;

public interface IEmployeeRepository : IRepository<Employee>,
    ISpecificationRepository<Employee>;