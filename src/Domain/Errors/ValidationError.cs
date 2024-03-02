using FluentResults;
using FluentValidation.Results;

namespace Domain.Errors;

public class ValidationError : Error
{
    public ValidationError(IEnumerable<ValidationFailure> failures)
    {
        Failures = failures;
    }

    public IEnumerable<ValidationFailure> Failures { get; }
}