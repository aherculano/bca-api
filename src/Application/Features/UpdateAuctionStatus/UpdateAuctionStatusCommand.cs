using FluentResults;
using MediatR;

namespace Application.Features.UpdateAuctionStatus;

public class UpdateAuctionStatusCommand : IRequest<Result<string>>
{
    public UpdateAuctionStatusCommand(Guid uniqueIdentifier, string auctionStatus)
    {
        UniqueIdentifier = uniqueIdentifier;
        AuctionStatus = auctionStatus;
    }

    public Guid UniqueIdentifier { get; }

    public string AuctionStatus { get; }
}