using Domain.Models.Auction;

namespace Application.Responses.AuctionResponses;

internal static class BidResponseMapper
{
    public static IEnumerable<BidResponse> MapToResponse(this IEnumerable<Bid> source)
    {
        if (source is null)
        {
            return null;
        }

        return source.Select(x => x.MapToResponse());
    }
    public static BidResponse MapToResponse(this Bid source)
    {
        if (source is null)
        {
            return null;
        }

        return new BidResponse(source.BidderName, source.BidValue);
    }
    
}