using Application.Requests.ListVehicleRequests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("v1/vehicles")]
[ApiController]
public class VehiclesController: ControllerBase
{
    private readonly IMediator _mediator;

    public VehiclesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{vehicleUniqueIdentifier}")]
    public async Task<IActionResult> GetVehicleByVehicleIdAsync(Guid vehicleUniqueIdentifier)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public async Task<IActionResult> ListVehiclesAsync([FromQuery] ListVehicleRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<IActionResult> CreateVehicleAsync()
    {
        throw new NotImplementedException();
    }
}