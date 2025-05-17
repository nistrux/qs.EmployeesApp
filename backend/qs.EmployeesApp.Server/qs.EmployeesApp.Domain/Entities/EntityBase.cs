namespace qs.EmployeesApp.Domain.Entities;

public abstract class EntityBase<TKey> where TKey : struct
{
    public TKey Id { get; set; }
}