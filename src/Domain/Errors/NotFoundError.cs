namespace Domain.Errors;

public class NotFoundError : ApplicationError
{
    public NotFoundError(string title, string details) : base(title, details)
    {
    }
}