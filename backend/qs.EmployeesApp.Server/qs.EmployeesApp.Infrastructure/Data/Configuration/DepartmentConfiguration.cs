using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using qs.EmployeesApp.Domain.Entities;

namespace qs.EmployeesApp.Infrastructure.Data.Configuration;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public const int NameMaxLength = 128;

    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id).ValueGeneratedOnAdd();

        builder.Property(d => d.Name).IsRequired().HasMaxLength(NameMaxLength);

        builder.HasMany(d => d.Employees)
            .WithOne(e => e.Department)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}