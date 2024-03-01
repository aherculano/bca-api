using Application.Errors;
using Application.FluentResults;
using Application.Responses.AuctionResponses;
using Domain.Models.Auction;
using Domain.Repositories;
using FluentResults;
using MediatR;

namespace Application.Features.CreateAuction;

public class CreateAuctionCommandHandler : IRequestHandler<CreateAuctionCommand, Result<AuctionResponse>>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IAuctionRepository _auctionRepository;

    public CreateAuctionCommandHandler(
        IVehicleRepository vehicleRepository,
        IAuctionRepository auctionRepository)
    {
        _vehicleRepository = vehicleRepository;
        _auctionRepository = auctionRepository;
    }
    
    public async Task<Result<AuctionResponse>> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
    {
        var vehicleResult = await _vehicleRepository
            .GetVehicleByUniqueIdentifierAsync(request.VehicleUniqueIdentifier);

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

        return Result.Ok(createResult.Value.MapToResponse());
    }
}