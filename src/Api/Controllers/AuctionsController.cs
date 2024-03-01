using Api.FluentResultExtensions;
using Api.Requests;
using Api.Responses;
using Application.Features.CreateAuction;
using Application.Features.GetAuctionByUniqueIdentifier;
using Application.Features.UpdateAuctionStatus;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("v1/auctions")]
[ApiController]
public class AuctionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuctionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAuctionAsync([FromBody] CreateAuctionRequest request)
    {
        var command = new CreateAuctionCommand(request.VehicleUniqueIdentifier);
        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? CreatedAtAction("GetAuctionByAuctionId", new { auctionUniqueIdentifier = result.Value.UniqueIdentifier },
                result.Value)
            : result.ToFailedActionResult();
    }

    [HttpGet("{auctionUniqueIdentifier}")]
    public async Task<IActionResult> GetAuctionByAuctionIdAsync([FromRoute] Guid auctionUniqueIdentifier)
    {
        var query = new GetAuctionByUniqueIdentifierQuery(auctionUniqueIdentifier);
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToFailedActionResult();
    }

    [HttpPut]
    [Route("{auctionUniqueIdentifier}/status")]
    public async Task<IActionResult> UpdateAuctionStatusAsync([FromRoute] Guid auctionUniqueIdentifier,
        [FromBody] UpdateAuctionStatusRequest request)
    {
        var command = new UpdateAuctionStatusCommand(auctionUniqueIdentifier, request.AuctionStatus);
        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? Ok(new AuctionStatusResponse(result.Value))
            : result.ToFailedActionResult();
    }
}