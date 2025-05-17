using qs.EmployeesApp.Domain.Entities;

namespace qs.EmployeesApp.Application.Contracts.Repositories;

public interface IPositionRepository: IRepository<Position>,
    ISpecificationRepository<Position>;