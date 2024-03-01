namespace Domain.Models.Auction;

public class Bid
{
    public string BidderName { get; }
    
    public decimal BidValue { get; }

    public Bid(string bidderName,
        decimal bidValue)
    {
        BidderName = bidderName;
        BidValue = bidValue;
    }
}