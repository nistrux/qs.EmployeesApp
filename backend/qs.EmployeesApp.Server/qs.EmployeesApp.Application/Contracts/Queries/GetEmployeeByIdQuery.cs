using MediatR;
using qs.EmployeesApp.Application.Contracts.DTO.Entities;

namespace qs.EmployeesApp.Application.Contracts.Queries;

public class GetEmployeeByIdQuery : IRequest<EmployeeDto?>
{
    public long Id { get; }

    public GetEmployeeByIdQuery(long id)
    {
        Id = id;
    }
}