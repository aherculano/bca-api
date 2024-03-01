using FluentResults;

namespace Application.Errors;

public abstract class ApplicationError : Error
{
    public string Title { get; set; }
    
    public string Details { get; set; }
    
    public ApplicationError(string title, string details)
    {
        Title = title;
        Details = details;
    }
}