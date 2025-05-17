using qs.EmployeesApp.Application.Contracts.DTO.Commands;
using qs.EmployeesApp.Application.Contracts.DTO.Entities;
using qs.EmployeesApp.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace qs.EmployeesApp.Application.Contracts.DTO.Mappers;

[Mapper]
public static partial class PositionMapper
{
    [MapperIgnoreTarget(nameof(Position.Id))]
    [MapperIgnoreTarget(nameof(Position.Employees))]
    public static partial Position MapToPosition(this CreatePositionDto dto);
    
    [MapperIgnoreSource(nameof(Position.Employees))]
    public static partial PositionDto MapToPositionDto(this Position obj);
}