namespace Domain.Errors;

public class ConflictAuctionError : ApplicationError
{
    public ConflictAuctionError(string title, string details) : base(title, details)
    {
    }
}