using MediatR;
using qs.EmployeesApp.Application.Contracts.Repositories;

namespace qs.EmployeesApp.Application.Contracts.Commands;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, bool>
{
    private readonly IEmployeeRepository _employeeRepository;

    public UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<bool> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (employee is null) return false;

        employee.Name = request.EmployeeDetails.Name;
        employee.DateOfBirth = request.EmployeeDetails.DateOfBirth;
        employee.EmploymentDate = request.EmployeeDetails.EmploymentDate;
        employee.SalaryAdjustment = request.EmployeeDetails.SalaryAdjustment;
        employee.PositionId = request.EmployeeDetails.PositionId;
        employee.DepartmentId = request.EmployeeDetails.DepartmentId;
        
        await _employeeRepository.UpdateAsync(employee, cancellationToken);
        
        return true;
    }
}