namespace qs.EmployeesApp.Application.Contracts.DTO.Queries;

public record PaginatedRequestDto(
    int PageNumber = 1,
    int PageSize = 10,
    string? SortField = null,
    string? SortOrder = null,
    string? FilterField = null,
    string? FilterValue = null);