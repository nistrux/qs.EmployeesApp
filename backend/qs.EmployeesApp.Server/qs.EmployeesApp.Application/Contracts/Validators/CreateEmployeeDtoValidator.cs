using FluentValidation;
using qs.EmployeesApp.Application.Contracts.DTO.Commands;
using qs.EmployeesApp.Application.Contracts.Repositories;

namespace qs.EmployeesApp.Application.Contracts.Validators;

// ReSharper disable once UnusedType.Global
public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
{
    public CreateEmployeeDtoValidator(IPositionRepository positionRepository,
        IDepartmentRepository departmentRepository)
    {
        RuleFor(x => x.Name).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(1, 128);
        
        RuleFor(x => x.SalaryAdjustment).Cascade(CascadeMode.Stop)
            .InclusiveBetween(-3_000, 90_000);

        RuleFor(x => x.DateOfBirth)
            .NotEmpty();

        RuleFor(x => x.EmploymentDate)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));

        RuleFor(x => x.PositionId).Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .PositionExists(positionRepository);

        RuleFor(x => x.DepartmentId).Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .DepartmentExists(departmentRepository);
    }
}