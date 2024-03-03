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

    public Bid HighestBid => Bids.OrderByDescending(x => x.BidValue).LastOrDefault();

    public bool AddBid(Bid bid)
    {
        if (Status is AuctionStatus.Closed)
        {
            return false;
        }

        if (HighestBid is not null && bid.BidValue > HighestBid.BidValue)
        {
            Bids.Add(bid);
            return true;
        }
        
        if (HighestBid is null && bid.BidValue > StartingBid)
        {
            Bids.Add(bid);
            return true;
        }

        return false;
    }

    public bool OpenAuction()
    {
        if (Status is AuctionStatus.Closed && !Bids.Any())
        {
            Status = AuctionStatus.Open;
            return true;
        }

        return false;
    }

    public bool CloseAuction()
    {
        if (Status is AuctionStatus.Open)
        {
            Status = AuctionStatus.Closed;
            return true;
        }

        return false;
    }
}