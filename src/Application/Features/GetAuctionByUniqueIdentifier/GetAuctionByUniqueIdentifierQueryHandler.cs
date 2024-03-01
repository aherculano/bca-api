using Application.Responses.AuctionResponses;
using Domain.Errors;
using Domain.FluentResults;
using Domain.Repositories;
using FluentResults;
using MediatR;

namespace Application.Features.GetAuctionByUniqueIdentifier;

public class GetAuctionByUniqueIdentifierQueryHandler : IRequestHandler<GetAuctionByUniqueIdentifierQuery, Result<AuctionResponse>>
{
    private readonly IAuctionRepository _auctionRepository;

    public GetAuctionByUniqueIdentifierQueryHandler(IAuctionRepository auctionRepository)
    {
        _auctionRepository = auctionRepository;
    }
    
    public async Task<Result<AuctionResponse>> Handle(GetAuctionByUniqueIdentifierQuery request, CancellationToken cancellationToken)
    {
        var auctionResult = await _auctionRepository.GetAuctionByUniqueIdentifier(request.UniqueIdentifier);

        var auction = auctionResult.ThrowExceptionIfHasFailedResult().Value;

        return auction != null
            ? Result.Ok(auction.MapToResponse())
            : Result.Fail(new NotFoundError("Not Found", "The Auction Was Not Found"));
    }
}