using MediatR;
using qs.EmployeesApp.Application.Contracts.DTO.Entities;
using qs.EmployeesApp.Application.Contracts.DTO.Mappers;
using qs.EmployeesApp.Application.Contracts.Repositories;
using qs.EmployeesApp.Application.Contracts.Specifications;

namespace qs.EmployeesApp.Application.Contracts.Queries;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto?>
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<EmployeeDto?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new EmployeeWithDetailsSpecification(request.Id);
        var employee = await _employeeRepository.GetBySpecificationAsync(spec, cancellationToken);
        if (employee == null)
        {
            return null;
        }

        return employee.MapToEmployeeDto();
    }
}