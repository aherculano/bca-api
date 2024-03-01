namespace Domain.Errors;

public class ClosedAuctionError : ApplicationError
{
    public ClosedAuctionError(string title, string details) : base(title, details)
    {
    }
}