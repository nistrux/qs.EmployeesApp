using MediatR;
using qs.EmployeesApp.Application.Contracts.DTO.Commands;

namespace qs.EmployeesApp.Application.Contracts.Commands;

public class UpdateEmployeeCommand : IRequest<bool>
{
    public long Id { get; }
    public UpdateEmployeeDto EmployeeDetails { get; }

    public UpdateEmployeeCommand(long id, UpdateEmployeeDto employeeDetails)
    {
        Id = id;
        EmployeeDetails = employeeDetails;
    }
}