using Domain.Models.Auction;
using Domain.Repositories;
using FluentResults;

namespace Infrastructure.Repositories;

public class AuctionInMemoryRepository : IAuctionRepository
{
    private readonly IList<Auction> _auctions;

    public AuctionInMemoryRepository()
    {
        _auctions = new List<Auction>();
    }

    public async Task<Result<Auction>> CreateAuction(Auction auction)
    {
        _auctions.Add(auction);
        return Result.Ok(auction);
    }

    public async Task<Result<bool>> UpdateAuction(Auction auction)
    {
        var currentAuction = _auctions.FirstOrDefault(x => x.UniqueIdentifier == auction.UniqueIdentifier);

        currentAuction.Bids = auction.Bids;
        currentAuction.Status = auction.Status;

        return Result.Ok(true);
    }

    public async Task<Result<IEnumerable<Auction>>> GetAuctionsByVehicleUniqueIdentifier(Guid vehicleUniqueIdentifer)
    {
        var auctions = _auctions.Where(x => x.VehicleUniqueIdentifier == vehicleUniqueIdentifer);

        return Result.Ok(auctions);
    }

    public async Task<Result<Auction>> GetAuctionByUniqueIdentifier(Guid uniqueIdentifier)
    {
        var auction = _auctions.FirstOrDefault(x => x.UniqueIdentifier == uniqueIdentifier);

        return Result.Ok(auction);
    }
}