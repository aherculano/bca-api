using Domain.Models.Auction;

namespace Application.Requests.BidRequests;

internal static class BidRequestMapper
{
    public static Bid MapToDomain(this BidRequest source)
    {
        if (source is null)
        {
            return null;
        }

        return new Bid(source.BidderName, source.BidValue);
    }
}