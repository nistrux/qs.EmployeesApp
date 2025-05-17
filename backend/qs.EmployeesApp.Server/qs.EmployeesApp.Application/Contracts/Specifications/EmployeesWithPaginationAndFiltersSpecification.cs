using Microsoft.EntityFrameworkCore;
using qs.EmployeesApp.Application.Contracts.DTO.Queries;
using qs.EmployeesApp.Domain.Entities;

namespace qs.EmployeesApp.Application.Contracts.Specifications;

public class EmployeesWithPaginationAndFiltersSpecification : SpecificationBase<Employee>
{
    public EmployeesWithPaginationAndFiltersSpecification(PaginatedRequestDto request)
        : base(query =>
        {
            query = query.Include(e => e.Position);
            query = query.Include(e => e.Department);

            var filteredQuery = new EmployeesFilterSpecification(request.FilterField, request.FilterValue).ApplyTo(query);
            var sortedQuery = new EmployeesSortSpecification(request.SortField, request.SortOrder).ApplyTo(filteredQuery);
            var pagedQuery = sortedQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);
            return pagedQuery;
        })
    {
    }
}