using Domain.Models.Auction;
using FluentResults;

namespace Domain.Repositories;

public interface IAuctionRepository
{
    Task<Result<Auction>> CreateAuctionAsync(Auction auction);

    Task<Result<bool>> UpdateAuctionAsync(Auction auction);

    Task<Result<IEnumerable<Auction>>> GetAuctionsByVehicleUniqueIdentifierAsync(Guid vehicleUniqueIdentifer);

    Task<Result<Auction>> GetAuctionByUniqueIdentifierAsync(Guid uniqueIdentifier);
}