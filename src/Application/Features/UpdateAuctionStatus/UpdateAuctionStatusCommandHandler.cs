﻿using Domain.Models.Auction;
using Domain.Services.Auctions;
using FluentResults;
using MediatR;

namespace Application.Features.UpdateAuctionStatus;

public class UpdateAuctionStatusCommandHandler : IRequestHandler<UpdateAuctionStatusCommand, Result>
{
    private readonly IAuctionService _auctionService;

    public UpdateAuctionStatusCommandHandler(IAuctionService auctionService)
    {
        _auctionService = auctionService;
    }
    
    public async Task<Result> Handle(UpdateAuctionStatusCommand request, CancellationToken cancellationToken)
    {
        var result = await _auctionService
            .UpdateAuctionStatus(request.UniqueIdentifier, MapToAuctionStatus(request.AuctionStatus));

        return result.IsSuccess ? result.ToResult() : Result.Fail(result.Errors);
    }

    private AuctionStatus MapToAuctionStatus(string status)
    {
        var result = Enum.TryParse(typeof(AuctionStatus), status, out var parsed);

        return (AuctionStatus) parsed;
    }
}