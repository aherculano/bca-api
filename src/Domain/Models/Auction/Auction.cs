using Domain.Models.Auction.ValueObjects;

namespace Domain.Models.Auction;

public class Auction : Entity
{
    public Auction(
        Guid vehicleUniqueIdentifier,
        decimal startingBid)
    {
        UniqueIdentifier = Guid.NewGuid();
        VehicleUniqueIdentifier = vehicleUniqueIdentifier;
        StartingBid = startingBid;
        Status = AuctionStatus.Closed;
        Bids = new List<Bid>();
    }

    public Guid VehicleUniqueIdentifier { get; set; }

    public decimal StartingBid { get; set; }

    public AuctionStatus Status { get; set; }

    public IList<Bid> Bids { get; set; }
}