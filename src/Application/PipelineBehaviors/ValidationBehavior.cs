using Domain.Errors;
using FluentResults;
using FluentValidation;
using MediatR;

namespace Application.PipelineBehaviors;

public class ValidationBehavior<TRequest, T> : IPipelineBehavior<TRequest, Result<T>>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<Result<T>> Handle(TRequest request, RequestHandlerDelegate<Result<T>> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(failure => failure != null)
            .ToList();

        if (failures.Any())
        {
            var exception = new ValidationException(failures);
            return Result.Fail(new ValidationError(failures).CausedBy(exception));
        }

        return await next();
    }
}