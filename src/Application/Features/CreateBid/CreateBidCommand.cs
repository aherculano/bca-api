using Application.Requests.BidRequests;
using Application.Responses.AuctionResponses;
using FluentResults;
using MediatR;

namespace Application.Features.CreateBid;

public class CreateBidCommand : IRequest<Result<BidResponse>>
{
    public Guid UniqueIdentifier { get; }
    public BidRequest Request { get; }

    public CreateBidCommand(Guid uniqueIdentifier, BidRequest request)
    {
        UniqueIdentifier = uniqueIdentifier;
        Request = request;
    }
}