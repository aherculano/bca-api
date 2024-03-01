using Domain.Errors;
using Domain.FluentResults;
using Domain.Models.Auction;
using Domain.Repositories;
using FluentResults;

namespace Domain.Services.Auctions;

public class AuctionService : IAuctionService
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IVehicleRepository _vehicleRepository;

    public AuctionService(IVehicleRepository vehicleRepository,
        IAuctionRepository auctionRepository)
    {
        _vehicleRepository = vehicleRepository;
        _auctionRepository = auctionRepository;
    }

    public async Task<Result<Auction>> CreateAuction(Guid vehicleUniqueIdentifier)
    {
        var vehicleResult = await _vehicleRepository
            .GetVehicleByUniqueIdentifierAsync(vehicleUniqueIdentifier);

        var vehicle = vehicleResult.ThrowExceptionIfHasFailedResult().Value;

        if (vehicle is null) return Result.Fail(new NotFoundError("Not Found", "The Vehicle Does Not Exist"));

        var auctionsResult = await _auctionRepository.GetAuctionsByVehicleUniqueIdentifier(vehicle.UniqueIdentifier);

        var currentAuctions = auctionsResult.ThrowExceptionIfHasFailedResult().Value;

        if (currentAuctions?.Any(x => x.Status is AuctionStatus.Open) is true)
            return Result.Fail(new AlreadyExistsError("Conflict", "There Is An Ongoing Auction For The Vehicle"));

        var auction = new Auction(vehicle.UniqueIdentifier, vehicle.StartingBid);

        var createResult = await _auctionRepository.CreateAuction(auction);

        createResult.ThrowExceptionIfHasFailedResult();

        return Result.Ok(createResult.Value);
    }

    public async Task<Result<AuctionStatus>> UpdateAuctionStatus(Guid auctionUniqueIdentifier, AuctionStatus status)
    {
        var currentAuctionResult = await _auctionRepository.GetAuctionByUniqueIdentifier(auctionUniqueIdentifier);
        var currentAuction = currentAuctionResult.ThrowExceptionIfHasFailedResult().Value;

        if (currentAuction is null) return Result.Fail(new NotFoundError("Not Found", "Auction Does Not Exist"));

        if (currentAuction.Status == status)
            return Result.Fail(new ConflictAuctionError("Conflict", "Auction Is Already With The Specified Status"));

        return status == AuctionStatus.Closed ? await CloseAuction(currentAuction) : await OpenAuction(currentAuction);
    }

    private async Task<Result<AuctionStatus>> CloseAuction(Auction currentAuction)
    {
        currentAuction.Status = AuctionStatus.Closed;

        (await _auctionRepository.UpdateAuction(currentAuction)).ThrowExceptionIfHasFailedResult();

        return Result.Ok(AuctionStatus.Closed);
    }

    private async Task<Result<AuctionStatus>> OpenAuction(Auction currentAuction)
    {
        var availableAuctionsResult =
            await _auctionRepository.GetAuctionsByVehicleUniqueIdentifier(currentAuction.VehicleUniqueIdentifier);

        var availableAuctions = availableAuctionsResult.ThrowExceptionIfHasFailedResult().Value;

        if (availableAuctions.Any(x => x.Status == AuctionStatus.Open))
            return Result.Fail(new ConflictAuctionError("Conflict", "There Is An Ongoing Auction For The Vehicle"));

        currentAuction.Status = AuctionStatus.Open;

        (await _auctionRepository.UpdateAuction(currentAuction)).ThrowExceptionIfHasFailedResult();

        return Result.Ok(AuctionStatus.Open);
    }
}