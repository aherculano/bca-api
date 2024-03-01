using FluentResults;

namespace Application.Errors;

public class AlreadyExistsError : ApplicationError
{
    public AlreadyExistsError(string title, string details) : base(title, details)
    {
    }
}