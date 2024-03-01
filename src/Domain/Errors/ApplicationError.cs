using FluentResults;

namespace Domain.Errors;

public abstract class ApplicationError : Error
{
    public ApplicationError(string title, string details)
    {
        Title = title;
        Details = details;
    }

    public string Title { get; set; }

    public string Details { get; set; }
}