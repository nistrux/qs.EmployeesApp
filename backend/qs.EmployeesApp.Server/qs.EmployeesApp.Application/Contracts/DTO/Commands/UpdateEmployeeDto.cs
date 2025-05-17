namespace qs.EmployeesApp.Application.Contracts.DTO.Commands;

public record UpdateEmployeeDto(
    long Id,
    string Name,
    DateOnly DateOfBirth,
    DateOnly EmploymentDate,
    int SalaryAdjustment, 
    long PositionId,
    long DepartmentId);