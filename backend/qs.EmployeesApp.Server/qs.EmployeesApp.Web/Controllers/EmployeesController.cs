using MediatR;
using Microsoft.AspNetCore.Mvc;
using qs.EmployeesApp.Application.Contracts;
using qs.EmployeesApp.Application.Contracts.Commands;
using qs.EmployeesApp.Application.Contracts.DTO.Commands;
using qs.EmployeesApp.Application.Contracts.DTO.Entities;
using qs.EmployeesApp.Application.Contracts.DTO.Queries;
using qs.EmployeesApp.Application.Contracts.Queries;

namespace qs.EmployeesApp.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<EmployeesController>  _logger;

    public EmployeesController(IMediator mediator, ILogger<EmployeesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmployeeDto>> CreateEmployee([FromBody] CreateEmployeeDto createEmployeeDto)
    {
        var command = new CreateEmployeeCommand(createEmployeeDto);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetEmployeeById), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeDto>> GetEmployeeById(long id)
    {
        var query = new GetEmployeeByIdQuery(id);
        var result = await _mediator.Send(query);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateEmployee(long id, [FromBody] UpdateEmployeeDto updateEmployeeDto)
    {
        if (id != updateEmployeeDto.Id)
        {
            return BadRequest();
        }

        var command = new UpdateEmployeeCommand(id, updateEmployeeDto);
        bool result = await _mediator.Send(command);

        if (result)
        {
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteEmployee(long id)
    {
        var command = new DeleteEmployeeCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
        {
            _logger.LogInformation("requested not existing Employee deletion with id {id}", id);
        }

        return NoContent();
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<EmployeeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedList<EmployeeDto>>> GetPaginatedEmployees(
        [FromQuery] PaginatedRequestDto request)
    {
        var query = new GetPaginatedEmployeesQuery(request);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("seed")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SeedData([FromQuery] int count)
    {
        if (count <= 0)
        {
            return BadRequest("Count must be greater than zero.");
        }

        await _mediator.Send(new SeedRandomDataCommand(count));
        return Created();
    }
}