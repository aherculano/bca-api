using FluentResults;
using MediatR;

namespace Application.Features.UpdateAuctionStatus;

public class UpdateAuctionStatusCommand : IRequest<Result>
{
    public Guid UniqueIdentifier { get; }
    
    public string AuctionStatus { get; }

    public UpdateAuctionStatusCommand(Guid uniqueIdentifier, string auctionStatus)
    {
        UniqueIdentifier = uniqueIdentifier;
        AuctionStatus = auctionStatus;
    }
}