using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace qs.EmployeesApp.Infrastructure.Data;

public class EmpDbContext : DbContext
{
    public EmpDbContext(DbContextOptions<EmpDbContext> options) : base(options)
    {
    }
    
    public DbSet<Domain.Entities.Position> Positions { get; set; }
    public DbSet<Domain.Entities.Employee> Employees { get; set; }
    public DbSet<Domain.Entities.Department> Departments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder
            .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(builder);
    }
}