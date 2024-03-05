using Domain.Errors;
using Domain.FluentResults;
using Domain.Models.Auction;
using Domain.Models.Auction.Validators;
using Domain.Models.Auction.ValueObjects;
using Domain.Models.Auction.ValueObjects.Validators;
using Domain.Repositories;
using FluentResults;

namespace Domain.Services.Auctions;

public class AuctionService : IAuctionService
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly AuctionValidator _auctionValidator;
    private readonly BidValidator _bidValidator;

    public AuctionService(IVehicleRepository vehicleRepository,
        IAuctionRepository auctionRepository)
    {
        _vehicleRepository = vehicleRepository;
        _auctionRepository = auctionRepository;
        _bidValidator = new BidValidator();
        _auctionValidator = new AuctionValidator();
    }

    public async Task<Result<Auction>> CreateAuction(Guid vehicleUniqueIdentifier)
    {
        var vehicleResult = await _vehicleRepository
            .GetVehicleByUniqueIdentifierAsync(vehicleUniqueIdentifier);

        var vehicle = vehicleResult.ThrowExceptionIfHasFailedResult().Value;

        if (vehicle is null) return Result.Fail(new NotFoundError("Not Found", "The Vehicle Does Not Exist"));

        var auctionsResult =
            await _auctionRepository.GetAuctionsByVehicleUniqueIdentifierAsync(vehicle.UniqueIdentifier);

        var currentAuctions = auctionsResult.ThrowExceptionIfHasFailedResult().Value;

        if (currentAuctions?.Any(x => x.Status is AuctionStatus.Open) is true)
            return Result.Fail(new AlreadyExistsError("Conflict", "There Is An Ongoing Auction For The Vehicle"));

        if (currentAuctions?.Any(x => x.Status == AuctionStatus.Closed && x.Bids.Any()) is true)
            return Result.Fail(new ConflictAuctionError("Conflict", "The Vehicle Was Already Sold"));
        
        var auction = new Auction(vehicle.UniqueIdentifier, vehicle.StartingBid);

        var validationResult = _auctionValidator.Validate(auction);

        if (!validationResult.IsValid) return Result.Fail(new ValidationError(validationResult.Errors));
        var createResult = await _auctionRepository.CreateAuctionAsync(auction);

        createResult.ThrowExceptionIfHasFailedResult();

        return Result.Ok(createResult.Value);
    }

    public async Task<Result<Bid>> AddBid(Guid auctionUniqueIdentifier, Bid bid)
    {
        var validationResult = _bidValidator.Validate(bid);

        if (!validationResult.IsValid) return Result.Fail(new ValidationError(validationResult.Errors));

        var currentAuctionResult = await _auctionRepository.GetAuctionByUniqueIdentifierAsync(auctionUniqueIdentifier);

        var currentAuction = currentAuctionResult.ThrowExceptionIfHasFailedResult().Value;

        if (currentAuction is null) return Result.Fail(new NotFoundError("Not Found", "Auction Was Not Found"));

        var wasBidAdded = currentAuction.AddBid(bid);

        if (!wasBidAdded) return GetBidErrorResult(bid, currentAuction);


        var updateResult = await _auctionRepository.UpdateAuctionAsync(currentAuction);

        updateResult.ThrowExceptionIfHasFailedResult();

        return Result.Ok(bid);
    }

    public async Task<Result<AuctionStatus>> UpdateAuctionStatus(Guid auctionUniqueIdentifier, AuctionStatus status)
    {
        var currentAuctionResult = await _auctionRepository.GetAuctionByUniqueIdentifierAsync(auctionUniqueIdentifier);
        var currentAuction = currentAuctionResult.ThrowExceptionIfHasFailedResult().Value;

        if (currentAuction is null) return Result.Fail(new NotFoundError("Not Found", "Auction Does Not Exist"));

        if (currentAuction.Status == status)
            return Result.Fail(new ConflictAuctionError("Conflict", "Auction Is Already With The Specified Status"));

        return status == AuctionStatus.Closed ? await CloseAuction(currentAuction) : await OpenAuction(currentAuction);
    }

    private static Result<Bid> GetBidErrorResult(Bid bid, Auction currentAuction)
    {
        if (currentAuction.Status is AuctionStatus.Closed)
            return Result.Fail(new ClosedAuctionError("Auction Closed",
                "The Bid Was Not Placed Because The Auction Is Closed"));

        if (currentAuction.HighestBid?.BidValue >= bid.BidValue)
            return Result.Fail(new InvalidBidError("Bad Request", "Bid is lower then highest bid"));

        if (currentAuction.StartingBid > bid.BidValue)
            return Result.Fail(new InvalidBidError("Bad Request", "Bid is lower then starting bid"));
        return null;
    }

    private async Task<Result<AuctionStatus>> CloseAuction(Auction currentAuction)
    {
        currentAuction.CloseAuction();

        (await _auctionRepository.UpdateAuctionAsync(currentAuction)).ThrowExceptionIfHasFailedResult();

        return Result.Ok(AuctionStatus.Closed);
    }

    private async Task<Result<AuctionStatus>> OpenAuction(Auction currentAuction)
    {
        var availableAuctionsResult =
            await _auctionRepository.GetAuctionsByVehicleUniqueIdentifierAsync(currentAuction.VehicleUniqueIdentifier);

        var availableAuctions = availableAuctionsResult.ThrowExceptionIfHasFailedResult().Value;

        if (availableAuctions.Any(x => x.Status == AuctionStatus.Open))
            return Result.Fail(new ConflictAuctionError("Conflict", "There Is An Ongoing Auction For The Vehicle"));

        var isOpen = currentAuction.OpenAuction();

        if (!isOpen) return Result.Fail(new ClosedAuctionError("Bad Request", "Vehicle Was Already Sold"));

        (await _auctionRepository.UpdateAuctionAsync(currentAuction)).ThrowExceptionIfHasFailedResult();

        return Result.Ok(AuctionStatus.Open);
    }
}