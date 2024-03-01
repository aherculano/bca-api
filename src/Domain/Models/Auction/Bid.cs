namespace Domain.Models.Auction;

public class Bid
{
    public Bid(string bidderName,
        decimal bidValue)
    {
        BidderName = bidderName;
        BidValue = bidValue;
    }

    public string BidderName { get; }

    public decimal BidValue { get; }
}