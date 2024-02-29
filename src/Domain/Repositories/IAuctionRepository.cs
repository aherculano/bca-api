using Domain.Models.Auction;
using FluentResults;

namespace Domain.Repositories;

public interface IAuctionRepository
{
    Task<Result<Auction>> CreateAuction(Auction auction);

    Task<Result<bool>> UpdateAuction(Auction auction);

    Task<Result<Auction>> GetAuctionByVehicleUniqueIdentifier(Guid vehicleUniqueIdentifer);

    Task<Result<Auction>> GetAuctionByUniqueIdentifier(Guid uniqueIdentifier);
}