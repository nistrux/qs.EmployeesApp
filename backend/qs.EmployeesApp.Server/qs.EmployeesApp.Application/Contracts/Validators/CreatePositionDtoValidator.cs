using FluentValidation;
using qs.EmployeesApp.Application.Contracts.DTO.Commands;

namespace qs.EmployeesApp.Application.Contracts.Validators;

// ReSharper disable once UnusedType.Global
public class CreatePositionDtoValidator : AbstractValidator<CreatePositionDto>
{
    public CreatePositionDtoValidator()
    {
        RuleFor(x => x.Name).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(1, 128);

        RuleFor(x => x.BaseSalary).Cascade(CascadeMode.Stop)
            .GreaterThan(0U);
    }
}