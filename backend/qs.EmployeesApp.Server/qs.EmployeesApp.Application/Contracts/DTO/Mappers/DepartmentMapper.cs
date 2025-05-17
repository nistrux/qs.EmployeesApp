using qs.EmployeesApp.Application.Contracts.DTO.Commands;
using qs.EmployeesApp.Application.Contracts.DTO.Entities;
using qs.EmployeesApp.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace qs.EmployeesApp.Application.Contracts.DTO.Mappers;

[Mapper]
public static partial class DepartmentMapper
{
    [MapperIgnoreTarget(nameof(Department.Id))]
    [MapperIgnoreTarget(nameof(Department.Employees))]
    public static partial Department MapToDepartment(this CreateDepartmentDto dto);

    [MapperIgnoreSource(nameof(Position.Employees))]
    public static partial DepartmentDto MapToDepartmentDto(this Department obj);
}