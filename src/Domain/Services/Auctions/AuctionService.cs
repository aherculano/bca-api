using Domain.Errors;
using Domain.FluentResults;
using Domain.Models.Auction;
using Domain.Repositories;
using FluentResults;

namespace Domain.Services.Auctions;

public class AuctionService : IAuctionService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IAuctionRepository _auctionRepository;

    public AuctionService(IVehicleRepository vehicleRepository,
        IAuctionRepository auctionRepository)
    {
        _vehicleRepository = vehicleRepository;
        _auctionRepository = auctionRepository;
    }

    public Task<Result<Auction>> OpenAuction(Guid auctionUniqueIdentifier)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Auction>> CloseAuction(Guid auctionUniqueIdentifier)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Auction>> CreateAuction(Guid vehicleUniqueIdentifier)
    {
        var vehicleResult = await _vehicleRepository
            .GetVehicleByUniqueIdentifierAsync(vehicleUniqueIdentifier);

        var vehicle = vehicleResult.ThrowExceptionIfHasFailedResult().Value;
        
        if (vehicle is null)
        {
            return Result.Fail(new NotFoundError("Not Found", "The Vehicle Does Not Exist"));
        }

        var auctionsResult = await _auctionRepository.GetAuctionsByVehicleUniqueIdentifier(vehicle.UniqueIdentifier);

        var currentAuctions = auctionsResult.ThrowExceptionIfHasFailedResult().Value;

        if (currentAuctions?.Any(x => x.Status is AuctionStatus.Open) is true)
        {
            return Result.Fail(new AlreadyExistsError("Conflict", "There Is An Ongoing Auction For The Vehicle"));
        }

        var auction = new Auction(vehicle.UniqueIdentifier, vehicle.StartingBid);

        var createResult = await _auctionRepository.CreateAuction(auction);

        createResult.ThrowExceptionIfHasFailedResult();

        return Result.Ok(createResult.Value);
    }
}