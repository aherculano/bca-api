using Application.Responses.AuctionResponses;
using FluentResults;
using MediatR;

namespace Application.Features.GetAuctionByUniqueIdentifier;

public class GetAuctionByUniqueIdentifierQuery : IRequest<Result<AuctionResponse>>
{
    public GetAuctionByUniqueIdentifierQuery(Guid uniqueIdentifier)
    {
        UniqueIdentifier = uniqueIdentifier;
    }

    public Guid UniqueIdentifier { get; }
}