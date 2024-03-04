namespace Domain.Models.Auction.ValueObjects;

public class Bid : ValueObject
{
    public Bid(string bidderName,
        decimal bidValue)
    {
        BidderName = bidderName;
        BidValue = bidValue;
        BidDate = DateTimeOffset.UtcNow;
    }

    public string BidderName { get; protected set; }

    public decimal BidValue { get; protected set; }

    public DateTimeOffset BidDate { get; protected set; }
}