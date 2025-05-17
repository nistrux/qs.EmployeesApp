using FluentValidation;
using qs.EmployeesApp.Application.Contracts.DTO.Commands;

namespace qs.EmployeesApp.Application.Contracts.Validators;

// ReSharper disable once UnusedType.Global
public class CreateDepartmentDtoValidator : AbstractValidator<CreateDepartmentDto>
{
    public CreateDepartmentDtoValidator()
    {
        RuleFor(x => x.Name).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(1, 128);
    }
}