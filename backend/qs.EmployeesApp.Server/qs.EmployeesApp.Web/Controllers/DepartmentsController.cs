using MediatR;
using Microsoft.AspNetCore.Mvc;
using qs.EmployeesApp.Application.Contracts.Commands;
using qs.EmployeesApp.Application.Contracts.DTO.Commands;
using qs.EmployeesApp.Application.Contracts.DTO.Entities;
using qs.EmployeesApp.Application.Contracts.Queries;

namespace qs.EmployeesApp.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DepartmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IList<DepartmentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<DepartmentDto>>> GetAllDepartments()
    {
        var query = new GetAllDepartmentsQuery();
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DepartmentDto>> CreateDepartment([FromBody] CreateDepartmentDto createDepartmentDto)
    {
        var command = new CreateDepartmentCommand(createDepartmentDto);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetDepartmentById), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DepartmentDto>> GetDepartmentById(long id)
    {
        var query = new GetDepartmentByIdQuery(id);
        var result = await _mediator.Send(query);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}