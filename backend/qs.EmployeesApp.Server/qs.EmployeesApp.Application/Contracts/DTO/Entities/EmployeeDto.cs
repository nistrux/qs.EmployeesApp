namespace qs.EmployeesApp.Application.Contracts.DTO.Entities;

public record EmployeeDto(
    long Id,
    string Name,
    DateOnly DateOfBirth,
    DateOnly EmploymentDate,
    uint? Salary,
    int SalaryAdjustment,
    long PositionId,
    string? PositionName,
    long DepartmentId,
    string? DepartmentName);