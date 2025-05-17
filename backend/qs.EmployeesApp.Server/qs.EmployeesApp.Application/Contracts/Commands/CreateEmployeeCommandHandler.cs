using MediatR;
using qs.EmployeesApp.Application.Contracts.DTO.Entities;
using qs.EmployeesApp.Application.Contracts.DTO.Mappers;
using qs.EmployeesApp.Application.Contracts.Repositories;

namespace qs.EmployeesApp.Application.Contracts.Commands;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    private readonly IEmployeeRepository _employeeRepository;

    public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = request.EmployeeDetails.MapToEmployee();

        var addedEmployee = await _employeeRepository.AddAsync(employee, cancellationToken);

        return addedEmployee.MapToEmployeeDto();
    }
}