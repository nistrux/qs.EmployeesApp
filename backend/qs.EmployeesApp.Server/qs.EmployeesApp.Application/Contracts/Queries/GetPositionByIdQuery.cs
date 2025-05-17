using MediatR;
using qs.EmployeesApp.Application.Contracts.DTO.Entities;

namespace qs.EmployeesApp.Application.Contracts.Queries;

public class GetPositionByIdQuery : IRequest<PositionDto?>
{
    public long Id { get; }

    public GetPositionByIdQuery(long id)
    {
        Id = id;
    }
}