using Domain.Models.Auction;
using Domain.Models.Auction.ValueObjects;
using FluentResults;

namespace Domain.Services.Auctions;

public interface IAuctionService
{
    Task<Result<AuctionStatus>> UpdateAuctionStatus(Guid auctionUniqueIdentifier, AuctionStatus status);

    Task<Result<Auction>> CreateAuction(Guid vehicleUniqueIdentifier);

    Task<Result<Bid>> AddBid(Guid auctionUniqueIdentifier, Bid bid);
}