using FluentValidation;
using qs.EmployeesApp.Application.Contracts.Repositories;

namespace qs.EmployeesApp.Application.Contracts.Validators;

public static class PositionExistsValidator
{
    public static IRuleBuilderOptions<T, long> PositionExists<T>(this IRuleBuilder<T, long> ruleBuilder,
        IPositionRepository positionRepository)
    {
        return ruleBuilder.MustAsync(async (id, token) =>
        {
            var directoryExists =
                await positionRepository.GetByIdAsync(id, token);
            return directoryExists != null;
        }).WithMessage("The specified position does not exist.");
    }
}