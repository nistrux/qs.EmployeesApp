using MediatR;
using qs.EmployeesApp.Application.Contracts.DTO.Entities;
using qs.EmployeesApp.Application.Contracts.DTO.Mappers;
using qs.EmployeesApp.Application.Contracts.Repositories;
using qs.EmployeesApp.Application.Contracts.Specifications;

namespace qs.EmployeesApp.Application.Contracts.Queries;

public class GetPaginatedEmployeesQueryHandler : IRequestHandler<GetPaginatedEmployeesQuery, PaginatedList<EmployeeDto>>
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetPaginatedEmployeesQueryHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<PaginatedList<EmployeeDto>> Handle(GetPaginatedEmployeesQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new EmployeesWithPaginationAndFiltersSpecification(request.PaginatedRequestDto);
        var employees = await _employeeRepository.ListAsync(spec, cancellationToken);

        var totalCount = await _employeeRepository.CountAsync(
            new EmployeesFilterSpecification(request.PaginatedRequestDto.FilterField,
                request.PaginatedRequestDto.FilterValue), cancellationToken);

        var employeeDtos = employees.Select(EmployeeMapper.MapToEmployeeDto).ToList();

        return new PaginatedList<EmployeeDto>(employeeDtos, totalCount, request.PaginatedRequestDto.PageNumber,
            request.PaginatedRequestDto.PageSize);
    }
}