namespace Domain.Errors;

public class InvalidBidError : ApplicationError
{
    public InvalidBidError(string title, string details) : base(title, details)
    {
    }
}