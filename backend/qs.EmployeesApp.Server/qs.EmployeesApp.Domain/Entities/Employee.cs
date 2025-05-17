namespace qs.EmployeesApp.Domain.Entities;

public class Employee : EntityBase<long>
{
    public required string Name { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required DateOnly EmploymentDate { get; set; }
    
    public required long PositionId { get; set; }
    public Position? Position { get; set; }

    public int SalaryAdjustment { get; set; }
    
    public required long DepartmentId { get; set; }
    public Department? Department { get; set; }
}