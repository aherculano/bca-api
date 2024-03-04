using Api.FluentResultExtensions;
using Api.Requests;
using Api.Responses;
using Application.Features.CreateAuction;
using Application.Features.CreateBid;
using Application.Features.GetAuctionByUniqueIdentifier;
using Application.Features.UpdateAuctionStatus;
using Application.Requests.BidRequests;
using Application.Responses.AuctionResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AuctionStatusResponse = Api.Responses.AuctionStatusResponse;

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
    [ProducesResponseType(typeof(AuctionResponse), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 409)]
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
    [ProducesResponseType(typeof(AuctionResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<IActionResult> GetAuctionByAuctionIdAsync([FromRoute] Guid auctionUniqueIdentifier)
    {
        var query = new GetAuctionByUniqueIdentifierQuery(auctionUniqueIdentifier);
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToFailedActionResult();
    }

    [HttpPut]
    [ProducesResponseType(typeof(AuctionStatusResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 409)]
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

    [HttpPost]
    [ProducesResponseType(typeof(BidResponse), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 409)]
    [Route("{auctionUniqueIdentifier}/bids")]
    public async Task<IActionResult> CreateBidAsync([FromRoute] Guid auctionUniqueIdentifier,
        [FromBody] BidRequest request)
    {
        var command = new CreateBidCommand(auctionUniqueIdentifier, request);
        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? Created()
            : result.ToFailedActionResult();
    }
}