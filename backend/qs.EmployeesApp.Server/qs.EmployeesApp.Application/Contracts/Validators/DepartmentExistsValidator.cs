using FluentValidation;
using qs.EmployeesApp.Application.Contracts.Repositories;

namespace qs.EmployeesApp.Application.Contracts.Validators;

public static class DepartmentExistsValidator
{
    public static IRuleBuilderOptions<T, long> DepartmentExists<T>(this IRuleBuilder<T, long> ruleBuilder,
        IDepartmentRepository departmentRepository)
    {
        return ruleBuilder.MustAsync(async (id, token) =>
        {
            var directoryExists =
                await departmentRepository.GetByIdAsync(id, token);
            return directoryExists != null;
        }).WithMessage("The specified department does not exist.");
    }
}