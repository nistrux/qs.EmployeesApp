namespace qs.EmployeesApp.Domain.Entities;

public class Department: EntityBase<long>
{
    public required string Name { get; set; }
    public List<Employee> Employees { get; set; } = new();
}