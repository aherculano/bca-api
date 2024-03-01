namespace Domain.Models.Auction;

public class Auction
{
    public Auction(
        Guid vehicleUniqueIdentifier,
        decimal startBid)
    {
        UniqueIdentifier = Guid.NewGuid();
        VehicleUniqueIdentifier = vehicleUniqueIdentifier;
        Status = AuctionStatus.Closed;
        StartBid = startBid;
        Bids = new List<Bid>();
    }

    public Guid UniqueIdentifier { get; set; }

    public Guid VehicleUniqueIdentifier { get; set; }

    public AuctionStatus Status { get; set; }

    public decimal StartBid { get; set; }

    public IList<Bid> Bids { get; set; }
}