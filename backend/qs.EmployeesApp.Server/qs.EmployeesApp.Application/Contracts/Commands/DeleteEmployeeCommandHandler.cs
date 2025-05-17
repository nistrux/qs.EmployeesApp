using MediatR;
using qs.EmployeesApp.Application.Contracts.Repositories;

namespace qs.EmployeesApp.Application.Contracts.Commands;

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, bool>
{
    private readonly IEmployeeRepository _employeeRepository;

    public DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (employee is null) return false;

        await _employeeRepository.DeleteAsync(employee, cancellationToken);

        return true;
    }
}