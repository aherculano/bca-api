using Domain.Models.Auction;
using Domain.Repositories;
using FluentResults;

namespace Infrastructure.Repositories;

public class AuctionInMemoryRepository : IAuctionRepository
{
    public Task<Result<Auction>> CreateAuction(Auction auction)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> UpdateAuction(Auction auction)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Auction>>> GetAuctionsByVehicleUniqueIdentifier(Guid vehicleUniqueIdentifer)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Auction>> GetAuctionByUniqueIdentifier(Guid uniqueIdentifier)
    {
        throw new NotImplementedException();
    }
}