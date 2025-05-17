using FluentValidation;
using qs.EmployeesApp.Application.Contracts.DTO.Queries;

namespace qs.EmployeesApp.Application.Contracts.Validators;

// ReSharper disable once UnusedType.Global
public class PaginatedRequestDtoValidator : AbstractValidator<PaginatedRequestDto>
{
    private readonly string[] _allowedSortFields = ["id", "name", "dateOfBirth", "employmentDate", "positionId", "departmentId", "salary"];
    private readonly string[] _allowedFilterFields = ["name", "position"];
    
    public PaginatedRequestDtoValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.SortField)
            .Must(value => _allowedSortFields.Contains(value, StringComparer.Ordinal))
            .When(x => !string.IsNullOrEmpty(x.SortOrder))
            .WithMessage(
                $"{nameof(PaginatedRequestDto.SortField)} required if {nameof(PaginatedRequestDto.SortOrder)} is not empty. Valid values are {string.Join(' ', _allowedSortFields)}");

        RuleFor(x => x.SortOrder)
            .Must(x => string.IsNullOrEmpty(x) || x.ToLower() == "asc" || x.ToLower() == "desc")
            .When(x => !string.IsNullOrEmpty(x.SortField) && string.IsNullOrEmpty(x.SortOrder))
            .WithMessage(
                $"{nameof(PaginatedRequestDto.SortOrder)} required if {nameof(PaginatedRequestDto.SortField)} is not empty. Valid values are 'asc' or 'desc'");

        RuleFor(x => x.FilterField)
            .Must(value => _allowedFilterFields.Contains(value, StringComparer.Ordinal))
            .When(x => !string.IsNullOrEmpty(x.FilterValue))
            .WithMessage(
                $"{nameof(PaginatedRequestDto.FilterField)} required if {nameof(PaginatedRequestDto.FilterValue)} is not empty. Valid values are {string.Join(' ', _allowedFilterFields)}");

        RuleFor(x => x.FilterValue)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.FilterField))
            .WithMessage(
                $"{nameof(PaginatedRequestDto.FilterValue)} required if {nameof(PaginatedRequestDto.FilterField)} is not empty");
    }
}