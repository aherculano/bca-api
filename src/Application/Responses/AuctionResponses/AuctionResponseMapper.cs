using Domain.Models.Auction;

namespace Application.Responses.AuctionResponses;

internal static class AuctionResponseMapper
{
    public static AuctionResponse MapToResponse(this Auction source)
    {
        if (source is null) return null;

        return new AuctionResponse(
            source.UniqueIdentifier,
            source.VehicleUniqueIdentifier,
            source.StartingBid,
            source.Status.ToString(),
            source.Bids.MapToResponse());
    }
}