using MediatR;
using Microsoft.AspNetCore.Mvc;
using qs.EmployeesApp.Application.Contracts.Commands;
using qs.EmployeesApp.Application.Contracts.DTO.Commands;
using qs.EmployeesApp.Application.Contracts.DTO.Entities;
using qs.EmployeesApp.Application.Contracts.Queries;

namespace qs.EmployeesApp.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PositionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PositionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IList<PositionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<PositionDto>>> GetAllPositions()
    {
        var query = new GetAllPositionsQuery();
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PositionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PositionDto>> CreatePosition([FromBody] CreatePositionDto createPositionDto)
    {
        var command = new CreatePositionCommand(createPositionDto);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetPositionById), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PositionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PositionDto>> GetPositionById(long id)
    {
        var query = new GetPositionByIdQuery(id);
        var result = await _mediator.Send(query);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}