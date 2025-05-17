using MediatR;
using qs.EmployeesApp.Application.Contracts.DTO.Entities;
using qs.EmployeesApp.Application.Contracts.DTO.Queries;

namespace qs.EmployeesApp.Application.Contracts.Queries;

public class GetPaginatedEmployeesQuery : IRequest<PaginatedList<EmployeeDto>>
{
    public readonly PaginatedRequestDto PaginatedRequestDto;

    public GetPaginatedEmployeesQuery(PaginatedRequestDto paginatedRequestDto)
    {
        PaginatedRequestDto = paginatedRequestDto with
        {
            FilterField = paginatedRequestDto.FilterField?.ToLowerInvariant(),
            SortOrder = paginatedRequestDto.SortOrder?.ToLowerInvariant() == "desc" ? "desc" : "asc"
        };
    }
}