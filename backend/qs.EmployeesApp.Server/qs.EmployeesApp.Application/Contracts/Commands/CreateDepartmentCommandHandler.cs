using MediatR;
using qs.EmployeesApp.Application.Contracts.DTO.Entities;
using qs.EmployeesApp.Application.Contracts.DTO.Mappers;
using qs.EmployeesApp.Application.Contracts.Repositories;

namespace qs.EmployeesApp.Application.Contracts.Commands;

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, DepartmentDto>
{
    private readonly IDepartmentRepository _departmentRepository;

    public CreateDepartmentCommandHandler(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<DepartmentDto> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var position = request.CreateDepartmentDto.MapToDepartment();

        var addedPosition = await _departmentRepository.AddAsync(position, cancellationToken);

        return addedPosition.MapToDepartmentDto();
    }
}