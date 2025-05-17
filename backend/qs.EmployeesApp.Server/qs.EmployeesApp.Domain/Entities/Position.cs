namespace qs.EmployeesApp.Domain.Entities;

public class Position : EntityBase<long>
{
    public required string Name { get; set; }
    public UInt32 BaseSalary { get; set; }
    public List<Employee> Employees { get; set; } = new();
}