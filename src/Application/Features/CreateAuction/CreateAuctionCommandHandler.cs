using Application.Responses.AuctionResponses;
using Domain.Errors;
using Domain.FluentResults;
using Domain.Models.Auction;
using Domain.Repositories;
using Domain.Services.Auctions;
using FluentResults;
using MediatR;

namespace Application.Features.CreateAuction;

public class CreateAuctionCommandHandler : IRequestHandler<CreateAuctionCommand, Result<AuctionResponse>>
{
    private readonly IAuctionService _auctionService;

    public CreateAuctionCommandHandler(IAuctionService auctionService)
    {
        _auctionService = auctionService;
    }
    
    public async Task<Result<AuctionResponse>> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
    {
        var result = await _auctionService.CreateAuction(request.VehicleUniqueIdentifier);
        return result.IsSuccess ? Result.Ok(result.Value.MapToResponse()) : Result.Fail(result.Errors); 
    }
}