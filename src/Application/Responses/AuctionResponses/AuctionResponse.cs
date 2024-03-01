namespace Application.Responses.AuctionResponses;

public record AuctionResponse(
    Guid UniqueIdentifier,
    Guid VehicleUniqueIdentifier,
    decimal StartingBid,
    string Status,
    IEnumerable<BidResponse> Bids);