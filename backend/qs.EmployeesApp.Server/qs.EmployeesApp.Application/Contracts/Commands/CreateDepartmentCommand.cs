using MediatR;
using qs.EmployeesApp.Application.Contracts.DTO.Commands;
using qs.EmployeesApp.Application.Contracts.DTO.Entities;

namespace qs.EmployeesApp.Application.Contracts.Commands;

public class CreateDepartmentCommand : IRequest<DepartmentDto>
{
    public CreateDepartmentDto CreateDepartmentDto { get; init; }

    public CreateDepartmentCommand(CreateDepartmentDto createDepartmentDto)
    {
        CreateDepartmentDto = createDepartmentDto;
    }
}