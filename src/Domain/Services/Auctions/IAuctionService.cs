using Domain.Models.Auction;
using FluentResults;

namespace Domain.Services.Auctions;

public interface IAuctionService
{
    Task<Result<AuctionStatus>> UpdateAuctionStatus(Guid auctionUniqueIdentifier, AuctionStatus status);

    Task<Result<Auction>> CreateAuction(Guid vehicleUniqueIdentifier);
}