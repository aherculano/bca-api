using Application.Responses.AuctionResponses;
using FluentResults;
using MediatR;

namespace Application.Features.CreateAuction;

public class CreateAuctionCommand : IRequest<Result<AuctionResponse>>
{
    public Guid VehicleUniqueIdentifier { get; }

    public CreateAuctionCommand(Guid vehicleUniqueIdentifier)
    {
        VehicleUniqueIdentifier = vehicleUniqueIdentifier;
    }
}