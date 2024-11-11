using EnterpriseHierarchyHub.Core.Domain;
using EnterpriseHierarchyHub.Core.Features.AddUnit;
using EnterpriseHierarchyHub.Core.Features.GetAllUnits;
using EnterpriseHierarchyHub.Core.Features.GetHeadUnit;
using EnterpriseHierarchyHub.Core.Features.GetUnitById;
using EnterpriseHierarchyHub.Core.Features.SearchUnits;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseHierarchyHub.API.Controllers;

[ApiController]
[Route("api/organization")]
public class OrganizationController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrganizationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddUnit([FromBody] OrganizationUnit unit)
    {
        var result = await _mediator.Send(new AddUnitCommand(unit));
        return Ok(result);
    }

    [HttpGet("childUnits")]
    public async Task<IActionResult> GetOrganizationUnitsByParent(
    [FromQuery] Guid? parentId,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetAllUnitsQuery(parentId, pageNumber, pageSize));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUnitById(Guid id)
    {
        var result = await _mediator.Send(new GetUnitByIdQuery(id));
        return Ok(result);
    }

    [HttpGet("headUnit")]
    public async Task<IActionResult> GetHead()
    {
        var result = await _mediator.Send(new GetHeadUnitQuery());
        return Ok(result);
    }

    [HttpGet("searchUnits")]
    public async Task<IActionResult> SearchUnits(
        [FromQuery] string name,
        [FromQuery] string type,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new SearchUnitsQuery(name, type, pageNumber, pageSize));
        return Ok(result);
    }
}