using Application.Requests.BidRequests;
using Application.Responses.AuctionResponses;
using Domain.Services.Auctions;
using FluentResults;
using MediatR;

namespace Application.Features.CreateBid;

public class CreateBidCommandHandler :IRequestHandler<CreateBidCommand, Result<BidResponse>>
{
    private readonly IAuctionService _auctionService;
    
    public CreateBidCommandHandler(IAuctionService auctionService)
    {
        _auctionService = auctionService;
    }
    
    public async Task<Result<BidResponse>> Handle(CreateBidCommand request, CancellationToken cancellationToken)
    {
        var bid = request.Request.MapToDomain();
        var bidResult = await _auctionService.AddBid(request.UniqueIdentifier, bid);

        return bidResult.IsSuccess ? Result.Ok(bidResult.Value.MapToResponse()) : Result.Fail(bidResult.Errors);
    }
}