using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using qs.EmployeesApp.Domain.Entities;

namespace qs.EmployeesApp.Infrastructure.Data.Configuration;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public const int NameMaxLength = 128;
    
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id).ValueGeneratedOnAdd();
        
        builder.Property(p => p.Name).IsRequired().HasMaxLength(NameMaxLength);

        builder.Property(p => p.BaseSalary).IsRequired();
    }
}