using Application.Responses.AuctionResponses;
using FluentResults;
using MediatR;

namespace Application.Features.CreateAuction;

public class CreateAuctionCommand : IRequest<Result<AuctionResponse>>
{
    public CreateAuctionCommand(Guid vehicleUniqueIdentifier)
    {
        VehicleUniqueIdentifier = vehicleUniqueIdentifier;
    }

    public Guid VehicleUniqueIdentifier { get; }
}