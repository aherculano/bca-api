using FluentResults;
using MediatR;

namespace Application.Features.UpdateAuctionStatus;

public class UpdateAuctionStatusCommand : IRequest<Result>
{
    public string AuctionStatus { get; }

    public UpdateAuctionStatusCommand(string auctionStatus)
    {
        AuctionStatus = auctionStatus;
    }
}