using MediatR;
using qs.EmployeesApp.Application.Contracts.DTO.Commands;
using qs.EmployeesApp.Application.Contracts.DTO.Entities;

namespace qs.EmployeesApp.Application.Contracts.Commands;

public class CreateEmployeeCommand: IRequest<EmployeeDto>
{
    public CreateEmployeeDto EmployeeDetails { get; init; }

    public CreateEmployeeCommand(CreateEmployeeDto employeeDetails)
    {
        EmployeeDetails = employeeDetails;
    }
}