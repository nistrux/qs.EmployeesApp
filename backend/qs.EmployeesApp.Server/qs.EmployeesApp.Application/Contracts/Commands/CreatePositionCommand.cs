using MediatR;
using qs.EmployeesApp.Application.Contracts.DTO.Commands;
using qs.EmployeesApp.Application.Contracts.DTO.Entities;

namespace qs.EmployeesApp.Application.Contracts.Commands;

public class CreatePositionCommand : IRequest<PositionDto>
{
    public CreatePositionDto CreatePositionDto { get; init; }

    public CreatePositionCommand(CreatePositionDto createPositionDto)
    {
        CreatePositionDto = createPositionDto;
    }
}