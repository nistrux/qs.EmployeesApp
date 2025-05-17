using MediatR;
using qs.EmployeesApp.Application.Contracts.DTO.Entities;

namespace qs.EmployeesApp.Application.Contracts.Queries;

public class GetAllDepartmentsQuery : IRequest<IReadOnlyList<DepartmentDto>>;