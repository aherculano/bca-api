using FluentResults;
using FluentValidation.Results;

namespace Application.Errors;

public class ValidationError : Error
{
    public ValidationError(IEnumerable<ValidationFailure> failures)
    {
        Failures = failures;
    }

    public IEnumerable<ValidationFailure> Failures { get; }
}