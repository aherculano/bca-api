using Application.Requests.BidRequests;
using Application.Responses.AuctionResponses;
using FluentResults;
using MediatR;

namespace Application.Features.CreateBid;

public class CreateBidCommand : IRequest<Result<BidResponse>>
{
    public CreateBidCommand(Guid uniqueIdentifier, BidRequest request)
    {
        UniqueIdentifier = uniqueIdentifier;
        Request = request;
    }

    public Guid UniqueIdentifier { get; }
    public BidRequest Request { get; }
}