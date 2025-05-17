using qs.EmployeesApp.Application.Contracts.DTO.Commands;
using qs.EmployeesApp.Application.Contracts.DTO.Entities;
using qs.EmployeesApp.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace qs.EmployeesApp.Application.Contracts.DTO.Mappers;

[Mapper]
public static partial class EmployeeMapper
{
    [MapperIgnoreTarget(nameof(Employee.Department))]
    [MapperIgnoreTarget(nameof(Employee.Position))]
    [MapperIgnoreTarget(nameof(Employee.Id))]
    public static partial Employee MapToEmployee(this CreateEmployeeDto dto);
    
    //[MapProperty("Position.BaseSalary", "Salary")]
    [MapPropertyFromSource(nameof(EmployeeDto.Salary), Use = nameof(MapSalaryWithAdjustment))]
    public static partial EmployeeDto MapToEmployeeDto(this Employee obj);

    private static uint? MapSalaryWithAdjustment(Employee emp)
    {
        if (emp.Position != null)
        {
            return (uint)(emp.Position.BaseSalary + emp.SalaryAdjustment);
        }

        return null;
    }
}