namespace qs.EmployeesApp.Application.Contracts.DTO.Commands;

public record CreateEmployeeDto(
    string Name,
    DateOnly DateOfBirth,
    DateOnly EmploymentDate,
    long PositionId,
    long DepartmentId,
    int SalaryAdjustment = 0);