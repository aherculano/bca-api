using Api.FluentResultExtensions;
using Api.Requests;
using Application.Features.GetVehicleByUniqueIdentifier;
using Application.Features.ListVehicles;
using Application.Requests.ListVehicleRequests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("v1/vehicles")]
[ApiController]
public class VehiclesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehiclesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{vehicleUniqueIdentifier}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetVehicleByVehicleIdAsync(Guid vehicleUniqueIdentifier)
    {
        var query = new GetVehicleByUniqueIdentifierQuery(vehicleUniqueIdentifier);
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToFailedActionResult();
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> ListVehiclesAsync([FromQuery] ListVehicleRequest request)
    {
        var query = new ListVehiclesQuery(request);
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToFailedActionResult();
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> CreateVehicleAsync([FromBody] CreateVehicleRequest request)
    {
        var command = request.MapToCommand();
        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? CreatedAtAction("GetVehicleByVehicleId",
                new { vehicleUniqueIdentifier = result.Value.UniqueIdentifier }, result.Value)
            : result.ToFailedActionResult();
    }
}