using Domain.Models.Auction;
using FluentResults;

namespace Domain.Services.Auctions;

public interface IAuctionService
{
    Task<Result<Auction>> OpenAuction(Guid auctionUniqueIdentifier);
    
    Task<Result<Auction>> CloseAuction(Guid auctionUniqueIdentifier);

    Task<Result<Auction>> CreateAuction(Guid vehicleUniqueIdentifier);
}