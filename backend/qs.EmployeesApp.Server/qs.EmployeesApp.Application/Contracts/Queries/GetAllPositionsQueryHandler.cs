using MediatR;
using qs.EmployeesApp.Application.Contracts.DTO.Entities;
using qs.EmployeesApp.Application.Contracts.DTO.Mappers;
using qs.EmployeesApp.Application.Contracts.Repositories;

namespace qs.EmployeesApp.Application.Contracts.Queries;

public class GetAllPositionsQueryHandler : IRequestHandler<GetAllPositionsQuery, IReadOnlyList<PositionDto>>
{
    private readonly IPositionRepository _positionRepository;

    public GetAllPositionsQueryHandler(IPositionRepository positionRepository)
    {
        _positionRepository = positionRepository;
    }

    public async Task<IReadOnlyList<PositionDto>> Handle(GetAllPositionsQuery request,
        CancellationToken cancellationToken)
    {
        var positions = await _positionRepository.ListAsync(cancellationToken);

        var result = positions.Select(PositionMapper.MapToPositionDto).ToList().AsReadOnly();

        return result;
    }
}