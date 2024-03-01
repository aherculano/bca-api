using Domain.Models.Auction;

namespace Application.Responses.AuctionResponses;

internal static class AuctionStatusResponseMapper
{
    public static AuctionStatusResponse MapToAuctionStatusResponse(this Auction auction)
    {
        if (auction is null) return null;

        return new AuctionStatusResponse(auction.Status.ToString());
    }
}