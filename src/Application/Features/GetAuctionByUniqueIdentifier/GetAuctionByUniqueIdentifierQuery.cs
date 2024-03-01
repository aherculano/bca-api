using Application.Responses.AuctionResponses;
using FluentResults;
using MediatR;

namespace Application.Features.GetAuctionByUniqueIdentifier;

public class GetAuctionByUniqueIdentifierQuery : IRequest<Result<AuctionResponse>>
{
    public Guid UniqueIdentifier { get; }

    public GetAuctionByUniqueIdentifierQuery(Guid uniqueIdentifier)
    {
        UniqueIdentifier = uniqueIdentifier;
    }
}