using MediatR;
using qs.EmployeesApp.Application.Contracts.DTO.Entities;

namespace qs.EmployeesApp.Application.Contracts.Queries;

public class GetAllPositionsQuery : IRequest<IReadOnlyList<PositionDto>>;