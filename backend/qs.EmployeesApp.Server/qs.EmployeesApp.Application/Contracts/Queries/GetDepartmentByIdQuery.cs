using MediatR;
using qs.EmployeesApp.Application.Contracts.DTO.Entities;

namespace qs.EmployeesApp.Application.Contracts.Queries;

public class GetDepartmentByIdQuery : IRequest<DepartmentDto?>
{
    public long Id { get; }

    public GetDepartmentByIdQuery(long id)
    {
        Id = id;
    }
}